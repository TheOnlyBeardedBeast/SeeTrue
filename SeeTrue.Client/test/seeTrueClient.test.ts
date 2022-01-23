import { SignupRequest, TokenPair } from '../src/index.d';
import { SeeTrueClient } from '../src/index';
import {
  isAuthResponse,
  isHealthResponse,
  isSettingsResponse,
  isUserResponse,
} from '../src/index.guard';
import mailhog from 'mailhog';

const client = new SeeTrueClient(
  'http://localhost:5000',
  'http://localhost:5000'
);

const mails = mailhog();

describe('health', () => {
  beforeAll(async () => {
    await mails.deleteAll();
  });

  it('checks app health', async () => {
    const result = await client.health();

    expect(result).not.toBeNull();
    expect(isHealthResponse(result)).toBe(true);
  });
});

describe('settings', () => {
  it('gets app settings', async () => {
    const result = await client.settings();

    expect(result).not.toBeNull();
    expect(isSettingsResponse(result)).toBe(true);
  });
});

describe('auth flow', () => {
  const user: SignupRequest = {
    email: `test${Date.now()}@user.dev`,
    password: '12345678',
    userMetaData: { Name: 'Test User' },
    language: 'en',
  };

  it('should register user', async () => {
    const result = await client.signup(user);

    expect(result).not.toBeNull();
    expect(isUserResponse(result)).toBe(true);
  });

  it('should confirm user', async () => {
    const messages = await mails.messages();

    expect(messages?.count).toBe(1);

    const token = messages?.items?.[0]?.html?.match(
      /(?<=(\"https:\/\/frontendurl\.com\/confirm\/))([^"]+)/g
    )?.[0];

    expect(token).not.toBe(null);

    const result = await client.verifySignup(token!);

    expect(result).not.toBe(null);
    expect(isAuthResponse(result)).toBe(true);
  });

  it('should login user', async () => {
    const response = await client.login({
      email: user.email,
      password: user.password,
    });

    expect(response).not.toBe(null);
    expect(isAuthResponse(response)).toBe(true);
  });

  it('should refresh user token', async () => {
    const response = await client.refresh();

    expect(response).not.toBeNull();
    expect(isAuthResponse(response)).toBe(true);
  });

  it('should get user data', async () => {
    const response = await client.user();

    expect(response).not.toBeNull();
    expect(isUserResponse(response)).toBe(true);
  });

  it('should update user name', async () => {
    const updateKey = 'testExtension';
    const updateValue = 'testData';
    const response = await client.updateUser({
      userMetaData: { [updateKey]: updateValue },
    });

    expect(response.userMetaData[updateKey]).toBe(updateValue);
    expect(isUserResponse(response)).toBe(true);
  });

  it('should update password', async () => {
    const newPassword = '12345678910';

    const response = await client.updateUser({
      password: '12345678910',
    });

    expect(isUserResponse(response)).toBe(true);

    user.password = newPassword;
  });

  it('should logout user', async () => {
    const tokensCopy = { ...client.tokens } as TokenPair;

    await client.logout();

    expect(client.tokens).toBeUndefined();

    await expect(client.refresh()).rejects.toThrowError();
    await expect(client.user()).rejects.toThrowError();

    client.tokens = tokensCopy;

    await expect(client.refresh()).rejects.toThrowError();
    await expect(client.user()).rejects.toThrowError();
  });

  it('should login with new password', async () => {
    const response = await client.login({
      email: user.email,
      password: user.password,
    });

    expect(response).not.toBe(null);
    expect(isAuthResponse(response)).toBe(true);
  });

  it('should update email', async () => {
    const newEmail = user.email + 'update';

    const response = await client.updateUser({
      email: newEmail,
    });

    expect(isUserResponse(response)).toBe(true);
    expect(response.emailChange).toBe(newEmail);

    user.email = newEmail;

    const messages = await mails.messages();

    expect(messages?.count).toBe(2);

    const token = messages?.items?.[0]?.html?.match(
      /(?<=(\"https:\/\/frontendurl\.com\/confirm-email\/))([^"]+)/g
    )?.[0]!;

    await expect(client.confirmEmail({ token })).resolves.toBe(undefined);

    const userResponse = await client.user();

    expect(userResponse).not.toBe(null);
    expect(isUserResponse(userResponse)).toBe(true);
    expect(userResponse.emailChange).toBe(null);

    client.logout();
  });

  it('should recover user account', async () => {
    await expect(client.recover({ email: user.email })).resolves.toBe(
      undefined
    );

    const messages = await mails.messages();

    expect(messages?.count).toBe(3);

    const token = messages?.items?.[0]?.html?.match(
      /(?<=(\"https:\/\/frontendurl\.com\/recover\/))([^"]+)/g
    )?.[0]!;

    expect(token).not.toBeNull();

    const response = await client.verifyRecovery(token);

    expect(isAuthResponse(response)).toBe(true);

    client.logout();
  });

  it('should login with magiclink', async () => {
    await expect(client.requestMagiclink({ email: user.email })).resolves.toBe(
      undefined
    );

    const messages = await mails.messages();

    expect(messages?.count).toBe(4);

    const token = messages?.items?.[0]?.html?.match(
      /(?<=(\"https:\/\/frontendurl\.com\/magiclink\/))([^"]+)/g
    )?.[0]!;

    expect(token).not.toBeNull();

    const response = await client.processMagiclink({ token });

    expect(isAuthResponse(response)).toBe(true);
  });
});

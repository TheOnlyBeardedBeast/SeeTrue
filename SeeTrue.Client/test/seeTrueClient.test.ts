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
});

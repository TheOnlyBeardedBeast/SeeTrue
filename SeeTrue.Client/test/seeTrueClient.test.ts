import { SignupRequest } from '../src/index.d';
import { SeeTrueClient } from '../src/index';
import {
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
  });
});

import { SignupRequest } from '../src/index.d';
import { SeeTrueClient } from '../src/index';
import {
  isHealthResponse,
  isSettingsResponse,
  isUserResponse,
} from '../src/index.guard';

const client = new SeeTrueClient(
  'http://localhost:5000',
  'http://localhost:5000'
);

describe('health', () => {
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
});

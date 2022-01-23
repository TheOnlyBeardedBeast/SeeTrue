import { fetch } from 'cross-fetch';
import join from 'url-join';
import { dateParser } from './utils';

import {
  RequestMagicLinkRequest,
  ProcessMagicLinkRequest,
  ConfirmEmailRequest,
  SettingsResponse,
  HealthResponse,
  RecoverRequest,
  InviteRequest,
  SignupRequest,
  UserResponse,
  AuthResponse,
  LoginRequest,
  RefreshRequest,
  UserUpdateRequest,
  TokenPair,
  TokenChangeAction,
  VerifySignupRequest,
  UserCredentials,
  VerifyRecoveryRequest,
  VerifyInviteRequestData,
  VerifyInviteRequest,
} from './index.d';

export enum Paths {
  HEALTH = 'health',
  SETTINGS = 'settings',
  SIGNUP = 'signup',
  VERIFY = 'verify',
  TOKEN = 'token',
  USER = 'user',
  LOGOUT = 'logout',
  CONFIRMEMAIL = 'confirm-email',
  RECOVER = 'recover',
  MAGICLINK = 'magiclink',
  INVITE = 'invite',
}

// TODO: use cross-fetch

export class SeeTrueClient {
  public readonly host: string;
  public readonly onTokenChange: TokenChangeAction | undefined;
  public readonly audince: string;

  private _tokens: TokenPair | undefined;
  public get tokens(): TokenPair | undefined {
    return this._tokens;
  }
  public set tokens(v: TokenPair | undefined) {
    this._tokens = v;
    this.onTokenChange?.(v);
  }

  /**
   * Setups the client
   */
  constructor(
    host: string,
    audience: string,
    onTokenChange?: TokenChangeAction
  ) {
    this.host = host;
    this.audince = audience;
    this.onTokenChange = onTokenChange;
  }

  /**
   * Healthcheck
   */
  public async health(): Promise<HealthResponse> {
    const response = await fetch(join(this.host, Paths.HEALTH), {
      method: 'GET',
    });

    if (response.status !== 200) {
      throw new Error('Failed to fetch');
    }

    const result: HealthResponse = await response.json();

    return result;
  }

  /**
   * Settings
   */
  public async settings(): Promise<SettingsResponse> {
    const response = await fetch(join(this.host, Paths.SETTINGS), {
      method: 'GET',
    });

    if (response.status !== 200) {
      throw new Error('Failed to fetch');
    }

    const result: SettingsResponse = await response.json();

    return result;
  }

  /**
   * Signup user
   */
  public async signup(data: SignupRequest): Promise<UserResponse> {
    const response = await fetch(join(this.host, Paths.SIGNUP), {
      method: 'POST',
      body: JSON.stringify(data),
      headers: {
        'Content-Type': 'application/json',
        'X-JWT-AUD': this.audince,
      },
    });

    if (response.status !== 200) {
      throw new Error('Failed to fetch');
    }

    const json = await response.text();

    const result = JSON.parse(json, dateParser);

    return result as UserResponse;
  }

  /**
   * Invite
   */
  public async invite(data: InviteRequest): Promise<void> {
    if (!this.tokens?.access_token) {
      throw new Error('No accesstoken');
    }

    const response = await fetch(join(this.host, Paths.INVITE), {
      method: 'POST',
      body: JSON.stringify(data),
      headers: {
        'Content-Type': 'application/json',
        'X-JWT-AUD': this.audince,
        authorization: `Bearer ${this.tokens.access_token}`,
      },
    });

    if (response.status !== 204) {
      throw new Error('Failed to fetch');
    }
  }

  /**
   * Confirm email
   */
  public async confirmEmail(data: ConfirmEmailRequest): Promise<void> {
    const response = await fetch(join(this.host, Paths.CONFIRMEMAIL), {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'X-JWT-AUD': this.audince,
      },
      body: JSON.stringify(data),
    });

    if (response.status !== 204) {
      throw new Error('Failed to fetch');
    }
  }

  /**
   * Raw token verfication for signup and recovery
   */
  public async verify(
    data: VerifyInviteRequest | VerifyRecoveryRequest | VerifySignupRequest
  ): Promise<AuthResponse> {
    const response = await fetch(join(this.host, Paths.VERIFY), {
      method: 'POST',
      body: JSON.stringify(data),
      headers: {
        'Content-Type': 'application/json',
        'X-JWT-AUD': this.audince,
      },
    });

    if (response.status !== 200) {
      console.log(response.status);
      throw new Error('Failed to fetch');
    }

    const json = await response.text();

    const result = JSON.parse(json, dateParser) as AuthResponse;

    this.tokens = {
      access_token: result.access_token,
      refresh_token: result.refresh_token,
    } as TokenPair;

    return result as AuthResponse;
  }

  /**
   * Handles signup verification
   * Uses the raw verify method
   */
  public async verifySignup(token: string): Promise<AuthResponse> {
    return this.verify({ type: 'signup', token } as VerifySignupRequest);
  }

  /**
   * Handles recovery verification
   * Uses the raw verify method
   */
  public async verifyRecovery(token: string): Promise<AuthResponse> {
    return this.verify({ type: 'recovery', token } as VerifyRecoveryRequest);
  }

  /**
   * Handles invite verification
   * Uses the raw verify method
   */
  public async verifyInvite(
    data: VerifyInviteRequestData
  ): Promise<AuthResponse> {
    return this.verify({
      type: 'invite',
      token: data.token,
      name: data.name,
      password: data.password,
    } as VerifyInviteRequest);
  }

  /**
   * Request a magiclink confirmation from a SeeTrue server
   */
  public async requestMagiclink(data: RequestMagicLinkRequest): Promise<void> {
    const response = await fetch(join(this.host, Paths.MAGICLINK), {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'X-JWT-AUD': this.audince,
      },
      body: JSON.stringify(data),
    });

    if (response.status !== 204) {
      throw new Error('Failed to fetch');
    }
  }

  /**
   * Process magiclink provided by a SeeTrue server
   */
  public async processMagiclink(
    data: ProcessMagicLinkRequest
  ): Promise<AuthResponse> {
    const response = await fetch(join(this.host, Paths.MAGICLINK, data.token), {
      method: 'GET',
      headers: {
        'X-JWT-AUD': this.audince,
      },
    });

    if (response.status !== 200) {
      console.log(response.status, response.statusText);
      throw new Error('Failed to fetch');
    }

    const json = await response.text();

    const result = JSON.parse(json, dateParser) as AuthResponse;

    this.tokens = {
      access_token: result.access_token,
      refresh_token: result.refresh_token,
    } as TokenPair;

    return result;
  }

  /**
   * Request password recovery
   */
  public async recover(data: RecoverRequest): Promise<void> {
    const response = await fetch(join(this.host, Paths.RECOVER), {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'X-JWT-AUD': this.audince,
      },
      body: JSON.stringify(data),
    });

    if (response.status !== 204) {
      throw new Error('Failed to fetch');
    }
  }

  /**
   * Exchange user credentials or a refresh token for access and refresh tokens from a SeeTrue server
   */
  public async token(
    data: LoginRequest | RefreshRequest
  ): Promise<AuthResponse> {
    const response = await fetch(join(this.host, Paths.TOKEN), {
      method: 'POST',
      body: JSON.stringify(data),
      headers: {
        'Content-Type': 'application/json',
        'X-JWT-AUD': this.audince,
      },
    });

    if (response.status !== 200) {
      throw new Error('Failed to fetch');
    }

    const json = await response.text();

    const result = JSON.parse(json, dateParser) as AuthResponse;

    this.tokens = {
      access_token: result.access_token,
      refresh_token: result.refresh_token,
    } as TokenPair;

    return result;
  }

  /**
   * Exchange user credentials for access and refresh tokens from a SeeTrue server
   * Encasulates the raw token request
   */
  public login(credentilas: UserCredentials): Promise<AuthResponse> {
    return this.token({
      ...credentilas,
      grant_type: 'password',
    } as LoginRequest);
  }

  /**
   * Exchange refresh token for access and refresh tokens from a SeeTrue server
   * Encasulates the raw token request
   */
  public async refresh(): Promise<AuthResponse> {
    if (!this.tokens?.refresh_token) {
      throw new Error('No refreshtoken');
    }

    return this.token({
      refresh_token: this.tokens?.refresh_token!,
      grant_type: 'refresh_token',
    } as RefreshRequest);
  }

  /**
   * Request the currently logged in user
   */
  public async user(): Promise<UserResponse> {
    if (!this.tokens?.access_token) {
      throw new Error('No accesstoken');
    }

    const response = await fetch(join(this.host, Paths.USER), {
      method: 'GET',
      headers: {
        'X-JWT-AUD': this.audince,
        authorization: `Bearer ${this.tokens.access_token}`,
      },
    });

    if (response.status !== 200) {
      throw new Error('Failed to fetch');
    }

    const json = await response.text();

    const result = JSON.parse(json, dateParser) as UserResponse;

    return result;
  }

  /**
   * Update user
   */
  public async updateUser(data: UserUpdateRequest): Promise<UserResponse> {
    if (!this.tokens?.access_token) {
      throw new Error('No accesstoken');
    }

    const response = await fetch(join(this.host, Paths.USER), {
      method: 'PUT',
      headers: {
        'X-JWT-AUD': this.audince,
        'Content-Type': 'application/json',
        authorization: `Bearer ${this.tokens.access_token}`,
      },
      body: JSON.stringify(data),
    });

    if (response.status !== 200) {
      throw new Error('Failed to fetch');
    }

    const json = await response.text();

    const result = JSON.parse(json, dateParser) as UserResponse;

    return result;
  }

  /**
   * Revokes all the refresh tokens connected to the given login, Revokes all the access tokens connected to the given login
   */
  public async logout(): Promise<void> {
    if (!this.tokens?.access_token) {
      throw new Error('No accesstoken');
    }

    const response = await fetch(join(this.host, Paths.LOGOUT), {
      method: 'POST',
      headers: {
        'X-JWT-AUD': this.audince,
        authorization: `Bearer ${this.tokens.access_token}`,
      },
    });

    if (response.status !== 204) {
      console.log(response.status);
      throw new Error('Failed to fetch');
    }

    this.tokens = undefined;
  }
}

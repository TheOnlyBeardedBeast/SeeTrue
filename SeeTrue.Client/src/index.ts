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
  VerifyRequest,
  UserResponse,
  AuthResponse,
  LoginRequest,
  RefreshRequest,
  UserUpdateRequest,
  TokenPair,
  TokenChangeAction,
  VerifySignupRequest,
  UserCredentials,
} from './index.d';

export enum Paths {
  HEALTH = 'health',
  SETTINGS = 'settings',
  SIGNUP = 'signup',
  VERIFY = 'verify',
  TOKEN = 'token',
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
  public async invite(data: InviteRequest): Promise<void> {}

  /**
   * Confirm email
   */
  public async confirmEmail(data: ConfirmEmailRequest): Promise<void> {}

  /**
   * Raw token verfication for signup and recovery
   */
  public async verify(data: VerifyRequest): Promise<AuthResponse> {
    const response = await fetch(join(this.host, Paths.VERIFY), {
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

    return result as AuthResponse;
  }

  /**
   * Handles signap verification
   * User the raw verify method
   */
  public async verifySignup(token: string): Promise<AuthResponse> {
    return this.verify({ type: 'signup', token } as VerifySignupRequest);
  }

  /**
   * Request a magiclink confirmation from a SeeTrue server
   */
  public async requestMagiclink(data: RequestMagicLinkRequest): Promise<void> {}

  /**
   * Process magiclink provided by a SeeTrue server
   */
  public async processMagiclink(
    data: ProcessMagicLinkRequest
  ): Promise<AuthResponse> {
    return {} as AuthResponse;
  }

  /**
   * Request password recovery
   */
  public async recover(data: RecoverRequest): Promise<void> {}

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
      console.log(await response.text());
      throw new Error('Failed to fetch');
    }

    const json = await response.text();

    const result = JSON.parse(json, dateParser);

    return result as AuthResponse;
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
  public async refresh(refresh_token: string): Promise<AuthResponse> {
    return this.token({
      refresh_token,
      grant_type: 'refresh_token',
    } as RefreshRequest);
  }

  /**
   * Request the currently logged in user
   */
  public async user(): Promise<UserResponse> {
    return {} as UserResponse;
  }

  /**
   * Update user
   */
  public async updateUser(data: UserUpdateRequest): Promise<UserResponse> {
    return {} as UserResponse;
  }

  /**
   * Revokes all the refresh tokens connected to the given login, Revokes all the access tokens connected to the given login
   */
  public async logout(): Promise<void> {}
}

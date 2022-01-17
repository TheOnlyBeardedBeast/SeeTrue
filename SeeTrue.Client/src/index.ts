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
} from './index.d';

// TODO: use cross-fetch

export class SeeTrueClient {
  public readonly host: string;
  public readonly onTokenChange: TokenChangeAction | undefined;

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
  constructor(host: string, onTokenChange: TokenChangeAction) {
    this.host = host;
    this.onTokenChange = onTokenChange;
  }

  /**
   * Healthcheck
   */
  public async health(): Promise<HealthResponse> {
    return {} as HealthResponse;
  }

  /**
   * Settings
   */
  public async Settings(): Promise<SettingsResponse> {
    return {} as SettingsResponse;
  }

  /**
   * Signup user
   */
  public async signup(data: SignupRequest): Promise<UserResponse> {
    return {} as UserResponse;
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
   * Handles token verfication for signup and recovery
   */
  public async verify(data: VerifyRequest): Promise<AuthResponse> {
    return {} as AuthResponse;
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
    return {} as AuthResponse;
  }

  /**
   * Exchange user credentials for access and refresh tokens from a SeeTrue server
   * Encasulates the raw token request
   */
  public async login(data: LoginRequest): Promise<AuthResponse> {
    return this.token(data);
  }

  /**
   * Exchange refresh token for access and refresh tokens from a SeeTrue server
   * Encasulates the raw token request
   */
  public async refresh(data: RefreshRequest): Promise<AuthResponse> {
    return this.token(data);
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

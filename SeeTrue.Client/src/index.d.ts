export type MetaData = Record<string, any>;

export type VerificationType = 'signup' | 'recovery';

export interface TokenPair {
  access_token: string;
  refresh_token: string;
}

export type TokenChangeAction = (tokenPair: TokenPair | undefined) => void;

export interface HealthResponse {
  name: string;
  version: number;
  description: string;
}

export interface SettingsResponse {
  signup_disabled: true;
  autoconfirm: true;
}

export interface SignupRequest {
  email: string;
  password: string;
  userMetaData: MetaData;
}

export interface UserResponse {
  instanceID: string;
  id: string;
  aud: string;
  role: string;
  email: string;
  language: string;
  confirmedAt: Date;
  invitedAt: Date;
  recoverySentAt: Date;
  emailChange: string;
  emailChangeSentAt: Date;
  lastSignInAt: Date;
  appMetaData: MetaData;
  userMetaData: MetaData;
  isSuperAdmin: boolean;
  createdAt: Date;
  updatedAt: Date;
}

export interface InviteRequest {
  email: string;
}

export interface ConfirmEmailRequest {
  token: string;
}

export interface VerifyRequest {
  type: VerificationType;
  token: string;
  password: string;
}

export interface AuthResponse {
  access_token: string;
  token_type: string;
  expires_in: bigint;
  refresh_token: string;
  user: UserResponse;
}

export interface RequestMagicLinkRequest {
  email: string;
}

export interface ProcessMagicLinkRequest {
  token: string;
}

export interface RecoverRequest {
  email: string;
}

export interface TokenRequest {
  grant_type: string;
  email: string;
  password: string;
  refresh_token: string;
}

export interface LoginRequest
  extends Pick<TokenRequest, 'grant_type' | 'email' | 'password'> {
  grant_type: 'password';
}

export interface RefreshRequest
  extends Pick<TokenRequest, 'grant_type' | 'refresh_token'> {
  grant_type: 'refresh';
}

export interface UserUpdateRequest {
  email: string;
  password: string;
  userMetaData: MetaData;
}

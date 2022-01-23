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
  signup_disabled: boolean;
  autoconfirm: boolean;
}

export interface SignupRequest {
  email: string;
  password: string;
  userMetaData: MetaData;
  language: string;
}

export interface UserResponse {
  instanceID: string;
  id: string;
  aud: string;
  role: string;
  email: string;
  language: string;
  confirmedAt: Date | null;
  invitedAt: Date | null;
  recoverySentAt: Date | null;
  emailChange: string | null;
  emailChangeSentAt: Date | null;
  lastSignInAt: Date | null;
  appMetaData: MetaData;
  userMetaData: MetaData;
  isSuperAdmin: boolean | null;
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
  token?: string;
  password?: string;
}

export interface VerifySignupRequest extends Pick<VerifyRequest, 'token'> {
  type: 'signup';
  token: string;
}

export interface VerifyRecoveryRequest extends Pick<VerifyRequest, 'token'> {
  type: 'recovery';
  token: string;
}

export interface AuthResponse {
  access_token: string;
  token_type: string;
  expires_in: number;
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
  grant_type: 'refresh_token';
}

export interface UserUpdateRequest {
  email?: string;
  password?: string;
  userMetaData?: MetaData;
}

export interface UserCredentials {
  email: string;
  password: string;
}

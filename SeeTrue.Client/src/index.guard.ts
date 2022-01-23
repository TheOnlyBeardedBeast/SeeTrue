/*
 * Generated type guards for "index.d.ts".
 * WARNING: Do not manually change this file.
 */
import {
  MetaData,
  VerificationType,
  TokenPair,
  HealthResponse,
  SettingsResponse,
  SignupRequest,
  UserResponse,
  InviteRequest,
  ConfirmEmailRequest,
  AuthResponse,
  RequestMagicLinkRequest,
  ProcessMagicLinkRequest,
  RecoverRequest,
  TokenRequest,
  LoginRequest,
  RefreshRequest,
  UserUpdateRequest,
} from './index.d';

export function isMetaData(obj: any, _argumentName?: string): obj is MetaData {
  return (
    ((obj !== null && typeof obj === 'object') || typeof obj === 'function') &&
    Object.entries<any>(obj).every(([key, _value]) => typeof key === 'string')
  );
}

export function isVerificationType(
  obj: any,
  _argumentName?: string
): obj is VerificationType {
  return obj === 'signup' || obj === 'recovery';
}

export function isTokenPair(
  obj: any,
  _argumentName?: string
): obj is TokenPair {
  return (
    ((obj !== null && typeof obj === 'object') || typeof obj === 'function') &&
    typeof obj.access_token === 'string' &&
    typeof obj.refresh_token === 'string'
  );
}

export function isHealthResponse(
  obj: any,
  _argumentName?: string
): obj is HealthResponse {
  return (
    ((obj !== null && typeof obj === 'object') || typeof obj === 'function') &&
    typeof obj.name === 'string' &&
    typeof obj.version === 'number' &&
    typeof obj.description === 'string'
  );
}

export function isSettingsResponse(
  obj: any,
  _argumentName?: string
): obj is SettingsResponse {
  return (
    ((obj !== null && typeof obj === 'object') || typeof obj === 'function') &&
    typeof obj.signup_disabled === 'boolean' &&
    typeof obj.autoconfirm === 'boolean'
  );
}

export function isSignupRequest(
  obj: any,
  _argumentName?: string
): obj is SignupRequest {
  return (
    ((obj !== null && typeof obj === 'object') || typeof obj === 'function') &&
    typeof obj.email === 'string' &&
    typeof obj.password === 'string' &&
    (isMetaData(obj.userMetaData) as boolean) &&
    typeof obj.language === 'string'
  );
}

export function isUserResponse(
  obj: any,
  _argumentName?: string
): obj is UserResponse {
  return (
    ((obj !== null && typeof obj === 'object') || typeof obj === 'function') &&
    typeof obj.instanceID === 'string' &&
    typeof obj.id === 'string' &&
    typeof obj.aud === 'string' &&
    typeof obj.role === 'string' &&
    typeof obj.email === 'string' &&
    typeof obj.language === 'string' &&
    (obj.confirmedAt === null || obj.confirmedAt instanceof Date) &&
    (obj.invitedAt === null || obj.invitedAt instanceof Date) &&
    (obj.recoverySentAt === null || obj.recoverySentAt instanceof Date) &&
    (obj.emailChange === null || typeof obj.emailChange === 'string') &&
    (obj.emailChangeSentAt === null || obj.emailChangeSentAt instanceof Date) &&
    (obj.lastSignInAt === null || obj.lastSignInAt instanceof Date) &&
    (isMetaData(obj.appMetaData) as boolean) &&
    (isMetaData(obj.userMetaData) as boolean) &&
    (obj.isSuperAdmin === null ||
      obj.isSuperAdmin === false ||
      obj.isSuperAdmin === true) &&
    obj.createdAt instanceof Date &&
    obj.updatedAt instanceof Date
  );
}

export function isInviteRequest(
  obj: any,
  _argumentName?: string
): obj is InviteRequest {
  return (
    ((obj !== null && typeof obj === 'object') || typeof obj === 'function') &&
    typeof obj.email === 'string'
  );
}

export function isConfirmEmailRequest(
  obj: any,
  _argumentName?: string
): obj is ConfirmEmailRequest {
  return (
    ((obj !== null && typeof obj === 'object') || typeof obj === 'function') &&
    typeof obj.token === 'string'
  );
}

export function isAuthResponse(
  obj: any,
  _argumentName?: string
): obj is AuthResponse {
  return (
    ((obj !== null && typeof obj === 'object') || typeof obj === 'function') &&
    typeof obj.access_token === 'string' &&
    typeof obj.token_type === 'string' &&
    typeof obj.expires_in === 'number' &&
    typeof obj.refresh_token === 'string' &&
    (isUserResponse(obj.user) as boolean)
  );
}

export function isRequestMagicLinkRequest(
  obj: any,
  _argumentName?: string
): obj is RequestMagicLinkRequest {
  return (
    ((obj !== null && typeof obj === 'object') || typeof obj === 'function') &&
    typeof obj.email === 'string'
  );
}

export function isProcessMagicLinkRequest(
  obj: any,
  _argumentName?: string
): obj is ProcessMagicLinkRequest {
  return (
    ((obj !== null && typeof obj === 'object') || typeof obj === 'function') &&
    typeof obj.token === 'string'
  );
}

export function isRecoverRequest(
  obj: any,
  _argumentName?: string
): obj is RecoverRequest {
  return (
    ((obj !== null && typeof obj === 'object') || typeof obj === 'function') &&
    typeof obj.email === 'string'
  );
}

export function isTokenRequest(
  obj: any,
  _argumentName?: string
): obj is TokenRequest {
  return (
    ((obj !== null && typeof obj === 'object') || typeof obj === 'function') &&
    typeof obj.grant_type === 'string' &&
    typeof obj.email === 'string' &&
    typeof obj.password === 'string' &&
    typeof obj.refresh_token === 'string'
  );
}

export function isLoginRequest(
  obj: any,
  _argumentName?: string
): obj is LoginRequest {
  return (
    ((obj !== null && typeof obj === 'object') || typeof obj === 'function') &&
    typeof obj.grant_type === 'string' &&
    typeof obj.email === 'string' &&
    typeof obj.password === 'string' &&
    obj.grant_type === 'password'
  );
}

export function isRefreshRequest(
  obj: any,
  _argumentName?: string
): obj is RefreshRequest {
  return (
    ((obj !== null && typeof obj === 'object') || typeof obj === 'function') &&
    typeof obj.grant_type === 'string' &&
    typeof obj.refresh_token === 'string' &&
    obj.grant_type === 'refresh'
  );
}

export function isUserUpdateRequest(
  obj: any,
  _argumentName?: string
): obj is UserUpdateRequest {
  return (
    ((obj !== null && typeof obj === 'object') || typeof obj === 'function') &&
    typeof obj.email === 'string' &&
    typeof obj.password === 'string' &&
    (isMetaData(obj.userMetaData) as boolean)
  );
}

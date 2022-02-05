[seetrue.client](../README.md) / [Modules](../modules.md) / types

# Module: types

## Table of contents

### Interfaces

- [AuthResponse](../interfaces/types.AuthResponse.md)
- [ConfirmEmailRequest](../interfaces/types.ConfirmEmailRequest.md)
- [HealthResponse](../interfaces/types.HealthResponse.md)
- [InviteRequest](../interfaces/types.InviteRequest.md)
- [LoginRequest](../interfaces/types.LoginRequest.md)
- [ProcessMagicLinkRequest](../interfaces/types.ProcessMagicLinkRequest.md)
- [RecoverRequest](../interfaces/types.RecoverRequest.md)
- [RefreshRequest](../interfaces/types.RefreshRequest.md)
- [RequestMagicLinkRequest](../interfaces/types.RequestMagicLinkRequest.md)
- [SettingsResponse](../interfaces/types.SettingsResponse.md)
- [SignupRequest](../interfaces/types.SignupRequest.md)
- [TokenPair](../interfaces/types.TokenPair.md)
- [TokenRequest](../interfaces/types.TokenRequest.md)
- [UserCredentials](../interfaces/types.UserCredentials.md)
- [UserResponse](../interfaces/types.UserResponse.md)
- [UserUpdateRequest](../interfaces/types.UserUpdateRequest.md)
- [VerifyInviteRequest](../interfaces/types.VerifyInviteRequest.md)
- [VerifyInviteRequestData](../interfaces/types.VerifyInviteRequestData.md)
- [VerifyRecoveryRequest](../interfaces/types.VerifyRecoveryRequest.md)
- [VerifySignupRequest](../interfaces/types.VerifySignupRequest.md)

### Type aliases

- [MetaData](types.md#metadata)
- [TokenChangeAction](types.md#tokenchangeaction)
- [UserChangeAction](types.md#userchangeaction)
- [VerificationType](types.md#verificationtype)

## Type aliases

### MetaData

Ƭ **MetaData**: `Record`<`string`, `any`\>

#### Defined in

[types.ts:1](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/types.ts#L1)

___

### TokenChangeAction

Ƭ **TokenChangeAction**: (`tokenPair`: [`TokenPair`](../interfaces/types.TokenPair.md) \| `undefined`) => `void`

#### Type declaration

▸ (`tokenPair`): `void`

##### Parameters

| Name | Type |
| :------ | :------ |
| `tokenPair` | [`TokenPair`](../interfaces/types.TokenPair.md) \| `undefined` |

##### Returns

`void`

#### Defined in

[types.ts:10](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/types.ts#L10)

___

### UserChangeAction

Ƭ **UserChangeAction**: (`userResponse`: [`UserResponse`](../interfaces/types.UserResponse.md) \| `undefined`) => `void`

#### Type declaration

▸ (`userResponse`): `void`

##### Parameters

| Name | Type |
| :------ | :------ |
| `userResponse` | [`UserResponse`](../interfaces/types.UserResponse.md) \| `undefined` |

##### Returns

`void`

#### Defined in

[types.ts:11](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/types.ts#L11)

___

### VerificationType

Ƭ **VerificationType**: ``"signup"`` \| ``"recovery"``

#### Defined in

[types.ts:3](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/types.ts#L3)

[seetrue.client](../README.md) / [Modules](../modules.md) / [client](../modules/client.md) / SeeTrueClient

# Class: SeeTrueClient

[client](../modules/client.md).SeeTrueClient

SeeTrue.API js client for browser and node

## Table of contents

### Constructors

- [constructor](client.SeeTrueClient.md#constructor)

### Properties

- [\_authenticatedUser](client.SeeTrueClient.md#_authenticateduser)
- [\_tokens](client.SeeTrueClient.md#_tokens)
- [audince](client.SeeTrueClient.md#audince)
- [host](client.SeeTrueClient.md#host)
- [onTokenChange](client.SeeTrueClient.md#ontokenchange)
- [onUserChange](client.SeeTrueClient.md#onuserchange)

### Accessors

- [authenticatedUser](client.SeeTrueClient.md#authenticateduser)
- [tokens](client.SeeTrueClient.md#tokens)

### Methods

- [confirmEmail](client.SeeTrueClient.md#confirmemail)
- [health](client.SeeTrueClient.md#health)
- [invite](client.SeeTrueClient.md#invite)
- [login](client.SeeTrueClient.md#login)
- [logout](client.SeeTrueClient.md#logout)
- [processMagiclink](client.SeeTrueClient.md#processmagiclink)
- [recover](client.SeeTrueClient.md#recover)
- [refresh](client.SeeTrueClient.md#refresh)
- [requestMagiclink](client.SeeTrueClient.md#requestmagiclink)
- [settings](client.SeeTrueClient.md#settings)
- [signup](client.SeeTrueClient.md#signup)
- [silentTokenUpdate](client.SeeTrueClient.md#silenttokenupdate)
- [token](client.SeeTrueClient.md#token)
- [updateUser](client.SeeTrueClient.md#updateuser)
- [user](client.SeeTrueClient.md#user)
- [verify](client.SeeTrueClient.md#verify)
- [verifyInvite](client.SeeTrueClient.md#verifyinvite)
- [verifyRecovery](client.SeeTrueClient.md#verifyrecovery)
- [verifySignup](client.SeeTrueClient.md#verifysignup)

## Constructors

### constructor

• **new SeeTrueClient**(`host`, `audience`, `onTokenChange?`, `onUserChange?`)

Setups the client

#### Parameters

| Name | Type |
| :------ | :------ |
| `host` | `string` |
| `audience` | `string` |
| `onTokenChange?` | [`TokenChangeAction`](../modules/types.md#tokenchangeaction) |
| `onUserChange?` | [`UserChangeAction`](../modules/types.md#userchangeaction) |

#### Defined in

[client.ts:102](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L102)

## Properties

### \_authenticatedUser

• `Private` **\_authenticatedUser**: `undefined` \| [`UserResponse`](../interfaces/types.UserResponse.md)

Authenticated user

#### Defined in

[client.ts:84](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L84)

___

### \_tokens

• `Private` **\_tokens**: `undefined` \| [`TokenPair`](../interfaces/types.TokenPair.md)

Access and Refresh token pair

#### Defined in

[client.ts:66](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L66)

___

### audince

• `Readonly` **audince**: `string`

Audience

#### Defined in

[client.ts:62](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L62)

___

### host

• `Readonly` **host**: `string`

SeeTrue.API instance host

#### Defined in

[client.ts:50](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L50)

___

### onTokenChange

• `Readonly` **onTokenChange**: `undefined` \| [`TokenChangeAction`](../modules/types.md#tokenchangeaction)

Callback to minotir token changes

#### Defined in

[client.ts:54](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L54)

___

### onUserChange

• `Readonly` **onUserChange**: `undefined` \| [`UserChangeAction`](../modules/types.md#userchangeaction)

Callback to monitor user changes

#### Defined in

[client.ts:58](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L58)

## Accessors

### authenticatedUser

• `get` **authenticatedUser**(): `undefined` \| [`UserResponse`](../interfaces/types.UserResponse.md)

Authenticated user getter

#### Returns

`undefined` \| [`UserResponse`](../interfaces/types.UserResponse.md)

#### Defined in

[client.ts:88](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L88)

• `set` **authenticatedUser**(`v`): `void`

Authenticated setter

#### Parameters

| Name | Type |
| :------ | :------ |
| `v` | `undefined` \| [`UserResponse`](../interfaces/types.UserResponse.md) |

#### Returns

`void`

#### Defined in

[client.ts:94](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L94)

___

### tokens

• `get` **tokens**(): `undefined` \| [`TokenPair`](../interfaces/types.TokenPair.md)

Access and Refresh token pair getter

#### Returns

`undefined` \| [`TokenPair`](../interfaces/types.TokenPair.md)

#### Defined in

[client.ts:70](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L70)

• `set` **tokens**(`v`): `void`

Access and Refresh token pair setter

#### Parameters

| Name | Type |
| :------ | :------ |
| `v` | `undefined` \| [`TokenPair`](../interfaces/types.TokenPair.md) |

#### Returns

`void`

#### Defined in

[client.ts:76](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L76)

## Methods

### confirmEmail

▸ **confirmEmail**(`data`): `Promise`<`void`\>

Confirm email

#### Parameters

| Name | Type |
| :------ | :------ |
| `data` | [`ConfirmEmailRequest`](../interfaces/types.ConfirmEmailRequest.md) |

#### Returns

`Promise`<`void`\>

#### Defined in

[client.ts:198](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L198)

___

### health

▸ **health**(): `Promise`<[`HealthResponse`](../interfaces/types.HealthResponse.md)\>

Healthcheck

#### Returns

`Promise`<[`HealthResponse`](../interfaces/types.HealthResponse.md)\>

#### Defined in

[client.ts:117](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L117)

___

### invite

▸ **invite**(`data`): `Promise`<`void`\>

Invite

#### Parameters

| Name | Type |
| :------ | :------ |
| `data` | [`InviteRequest`](../interfaces/types.InviteRequest.md) |

#### Returns

`Promise`<`void`\>

#### Defined in

[client.ts:175](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L175)

___

### login

▸ **login**(`credentilas`): `Promise`<[`AuthResponse`](../interfaces/types.AuthResponse.md)\>

Exchange user credentials for access and refresh tokens from a SeeTrue server
Encasulates the raw token request

#### Parameters

| Name | Type |
| :------ | :------ |
| `credentilas` | [`UserCredentials`](../interfaces/types.UserCredentials.md) |

#### Returns

`Promise`<[`AuthResponse`](../interfaces/types.AuthResponse.md)\>

#### Defined in

[client.ts:380](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L380)

___

### logout

▸ **logout**(): `Promise`<`void`\>

Revokes all the refresh tokens connected to the given login, Revokes all the access tokens connected to the given login

#### Returns

`Promise`<`void`\>

#### Defined in

[client.ts:463](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L463)

___

### processMagiclink

▸ **processMagiclink**(`data`): `Promise`<[`AuthResponse`](../interfaces/types.AuthResponse.md)\>

Process magiclink provided by a SeeTrue server

#### Parameters

| Name | Type |
| :------ | :------ |
| `data` | [`ProcessMagicLinkRequest`](../interfaces/types.ProcessMagicLinkRequest.md) |

#### Returns

`Promise`<[`AuthResponse`](../interfaces/types.AuthResponse.md)\>

#### Defined in

[client.ts:298](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L298)

___

### recover

▸ **recover**(`data`): `Promise`<`void`\>

Request password recovery

#### Parameters

| Name | Type |
| :------ | :------ |
| `data` | [`RecoverRequest`](../interfaces/types.RecoverRequest.md) |

#### Returns

`Promise`<`void`\>

#### Defined in

[client.ts:329](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L329)

___

### refresh

▸ **refresh**(): `Promise`<[`AuthResponse`](../interfaces/types.AuthResponse.md)\>

Exchange refresh token for access and refresh tokens from a SeeTrue server
Encasulates the raw token request

#### Returns

`Promise`<[`AuthResponse`](../interfaces/types.AuthResponse.md)\>

#### Defined in

[client.ts:391](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L391)

___

### requestMagiclink

▸ **requestMagiclink**(`data`): `Promise`<`void`\>

Request a magiclink confirmation from a SeeTrue server

#### Parameters

| Name | Type |
| :------ | :------ |
| `data` | [`RequestMagicLinkRequest`](../interfaces/types.RequestMagicLinkRequest.md) |

#### Returns

`Promise`<`void`\>

#### Defined in

[client.ts:280](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L280)

___

### settings

▸ **settings**(): `Promise`<[`SettingsResponse`](../interfaces/types.SettingsResponse.md)\>

Settings

#### Returns

`Promise`<[`SettingsResponse`](../interfaces/types.SettingsResponse.md)\>

#### Defined in

[client.ts:134](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L134)

___

### signup

▸ **signup**(`data`): `Promise`<[`UserResponse`](../interfaces/types.UserResponse.md)\>

Signup user

#### Parameters

| Name | Type |
| :------ | :------ |
| `data` | [`SignupRequest`](../interfaces/types.SignupRequest.md) |

#### Returns

`Promise`<[`UserResponse`](../interfaces/types.UserResponse.md)\>

#### Defined in

[client.ts:151](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L151)

___

### silentTokenUpdate

▸ **silentTokenUpdate**(`tokens`): `void`

Token update without invoking the onTokenChangeCallback

#### Parameters

| Name | Type |
| :------ | :------ |
| `tokens` | `Partial`<[`TokenPair`](../interfaces/types.TokenPair.md)\> |

#### Returns

`void`

#### Defined in

[client.ts:488](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L488)

___

### token

▸ **token**(`data`): `Promise`<[`AuthResponse`](../interfaces/types.AuthResponse.md)\>

Exchange user credentials or a refresh token for access and refresh tokens from a SeeTrue server

#### Parameters

| Name | Type |
| :------ | :------ |
| `data` | [`LoginRequest`](../interfaces/types.LoginRequest.md) \| [`RefreshRequest`](../interfaces/types.RefreshRequest.md) |

#### Returns

`Promise`<[`AuthResponse`](../interfaces/types.AuthResponse.md)\>

#### Defined in

[client.ts:347](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L347)

___

### updateUser

▸ **updateUser**(`data`): `Promise`<[`UserResponse`](../interfaces/types.UserResponse.md)\>

Update user

#### Parameters

| Name | Type |
| :------ | :------ |
| `data` | [`UserUpdateRequest`](../interfaces/types.UserUpdateRequest.md) |

#### Returns

`Promise`<[`UserResponse`](../interfaces/types.UserResponse.md)\>

#### Defined in

[client.ts:433](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L433)

___

### user

▸ **user**(): `Promise`<[`UserResponse`](../interfaces/types.UserResponse.md)\>

Request the currently logged in user

#### Returns

`Promise`<[`UserResponse`](../interfaces/types.UserResponse.md)\>

#### Defined in

[client.ts:405](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L405)

___

### verify

▸ **verify**(`data`): `Promise`<[`AuthResponse`](../interfaces/types.AuthResponse.md)\>

Raw token verfication for signup and recovery

#### Parameters

| Name | Type |
| :------ | :------ |
| `data` | [`VerifyInviteRequest`](../interfaces/types.VerifyInviteRequest.md) \| [`VerifyRecoveryRequest`](../interfaces/types.VerifyRecoveryRequest.md) \| [`VerifySignupRequest`](../interfaces/types.VerifySignupRequest.md) |

#### Returns

`Promise`<[`AuthResponse`](../interfaces/types.AuthResponse.md)\>

#### Defined in

[client.ts:216](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L216)

___

### verifyInvite

▸ **verifyInvite**(`data`): `Promise`<[`AuthResponse`](../interfaces/types.AuthResponse.md)\>

Handles invite verification
Uses the raw verify method

#### Parameters

| Name | Type |
| :------ | :------ |
| `data` | [`VerifyInviteRequestData`](../interfaces/types.VerifyInviteRequestData.md) |

#### Returns

`Promise`<[`AuthResponse`](../interfaces/types.AuthResponse.md)\>

#### Defined in

[client.ts:266](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L266)

___

### verifyRecovery

▸ **verifyRecovery**(`token`): `Promise`<[`AuthResponse`](../interfaces/types.AuthResponse.md)\>

Handles recovery verification
Uses the raw verify method

#### Parameters

| Name | Type |
| :------ | :------ |
| `token` | `string` |

#### Returns

`Promise`<[`AuthResponse`](../interfaces/types.AuthResponse.md)\>

#### Defined in

[client.ts:258](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L258)

___

### verifySignup

▸ **verifySignup**(`token`): `Promise`<[`AuthResponse`](../interfaces/types.AuthResponse.md)\>

Handles signup verification
Uses the raw verify method

#### Parameters

| Name | Type |
| :------ | :------ |
| `token` | `string` |

#### Returns

`Promise`<[`AuthResponse`](../interfaces/types.AuthResponse.md)\>

#### Defined in

[client.ts:250](https://github.com/TheOnlyBeardedBeast/SeeTrue/blob/3dbc6e2/SeeTrue.Client/src/client.ts#L250)

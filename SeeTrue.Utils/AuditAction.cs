using System;
namespace SeeTrue.API.Utils
{
    public enum AuditAction
    {
        LoginAction,
        LogoutAction,
        InviteAcceptedAction,
        UserSignedUpAction,
        UserInvitedAction,
        UserDeletedAction,
        UserModifiedAction,
        UserRecoveryRequestedAction,
        TokenRevokedAction,
        TokenRefreshedAction
    }
}

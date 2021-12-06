using System;
namespace SeeTrue.Utils
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

using System;

namespace SeeTrue.Infrastructure.Types
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

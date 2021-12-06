using System;
using System.Collections.Generic;

namespace SeeTrue.Utils
{
    public static class Constants
    {
        public static readonly Dictionary<AuditAction, AuditLogType> ActionLogTypeMap = new Dictionary<AuditAction, AuditLogType>{
            { AuditAction.LoginAction, AuditLogType.Account },
            { AuditAction.LogoutAction, AuditLogType.Account },
            { AuditAction.InviteAcceptedAction, AuditLogType.Account },
            { AuditAction.UserSignedUpAction, AuditLogType.Team },
            { AuditAction.UserInvitedAction, AuditLogType.Team },
            { AuditAction.UserDeletedAction, AuditLogType.Team },
            { AuditAction.TokenRevokedAction, AuditLogType.Token },
            { AuditAction.TokenRefreshedAction, AuditLogType.Token },
            { AuditAction.UserModifiedAction, AuditLogType.User },
            { AuditAction.UserRecoveryRequestedAction, AuditLogType.User }
        };
    }
}

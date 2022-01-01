using System;
using System.Threading.Tasks;
using SeeTrue.Models;
using SeeTrue.Utils.Services;

namespace SeeTrue.Tests
{
    public class FakeMailer : IMailService
    {
        public (string, string) mail;

        public Task NotifyEmailChange(User user, string email)
        {
            mail = (user.Email, email);

            return Task.CompletedTask;
        }

        public Task NotifyInviteUser(User user)
        {
            mail = (user.Email, user.ConfirmationToken);

            return Task.CompletedTask;
        }

        public Task NotifyMagicLink(User user, string token)
        {
            mail = (user.Email, token);

            return Task.CompletedTask;
        }

        public Task NotifyRecovery(User user)
        {
            mail = (user.Email, user.RecoveryToken);

            return Task.CompletedTask;
        }

        public Task Send(string address, string code)
        {
            mail = (address, code);

            return Task.CompletedTask;
        }
    }
}

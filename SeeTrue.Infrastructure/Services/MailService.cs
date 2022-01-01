using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using SeeTrue.Models;
using SeeTrue.Infrastructure.Utils;
namespace SeeTrue.Utils.Services
{
    public interface IMailService
    {
        Task NotifyConfirmation(User user);
        Task NotifyEmailChange(User user, string email);
        Task NotifyRecovery(User user);
        Task NotifyMagicLink(User user, string token);
        Task NotifyInviteUser(User user);
    }

    public class MailService : IMailService
    {
        protected readonly SmtpClient client;
        public MailService()
        {
            this.client = new SmtpClient(Env.SmtpHost, Env.SmtpPort);
            this.client.Credentials = new NetworkCredential(Env.SmtpUser, Env.SmtpPass);
        }

        public async Task NotifyConfirmation(User user)
        {

            MailAddress from = new("service@mailhog.example", "Confirm user registration");
            MailAddress to = new(user.Email);

            MailMessage message = new MailMessage(from, to)
            {
                Subject = "Passwordless Confirm",
                Body = $"<a href=\"http://localhost:5000/confirm/{user.ConfirmationToken}\">confirm</a>",
                IsBodyHtml = true
            };

            await Task.Run(() => client.Send(message));
        }

        public async Task NotifyEmailChange(User user, string email)
        {
            MailAddress from = new("service@mailhog.example", "Confirm email change");
            MailAddress to = new(user.Email);

            MailMessage message = new MailMessage(from, to)
            {
                Subject = "Passwordless Update",
                Body = $"<a href=\"http://localhost:5000/confirm-emailchange/{user.EmailChangeToken}\">confirm {user.EmailChangeToken}</a>",
                IsBodyHtml = true
            };

            await Task.Run(() => client.Send(message));
        }

        public async Task NotifyRecovery(User user)
        {
            MailAddress from = new("service@mailhog.example", "Confirm email change");
            MailAddress to = new(user.Email);

            MailMessage message = new MailMessage(from, to)
            {
                Subject = "Passwordless Recovery",
                Body = $"<a href=\"http://localhost:5000/verify/{user.RecoveryToken}\">confirm {user.RecoveryToken}</a>",
                IsBodyHtml = true
            };

            await Task.Run(() => client.Send(message));
        }

        public async Task NotifyMagicLink(User user, string token)
        {
            MailAddress from = new("service@mailhog.example", "Confirm email change");
            MailAddress to = new(user.Email);

            MailMessage message = new MailMessage(from, to)
            {
                Subject = "Passwordless MagicLink",
                Body = $"<a href=\"http://localhost:5000/verify/{token}\">magiclink {token}</a>",
                IsBodyHtml = true
            };

            await Task.Run(() => client.Send(message));
        }

        public async Task NotifyInviteUser(User user)
        {
            MailAddress from = new("service@mailhog.example", "Confirm user invitation");
            MailAddress to = new(user.Email);

            MailMessage message = new MailMessage(from, to)
            {
                Subject = "Passwordless Confirm",
                Body = $"<a href=\"http://localhost:5000/confirm/{user.ConfirmationToken}\">Confirm notification</a>",
                IsBodyHtml = true
            };

            await Task.Run(() => client.Send(message));
        }
    }
}

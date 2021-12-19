using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using SeeTrue.Models;

namespace SeeTrue.Utils.Services
{
    public interface IMailService
    {
        Task Send(string address, string code);
        Task NotifyEmailChange(User user, string email);
        Task NotifyRecovery(User user);
        Task NotifyMagicLink(User user, string token);
    }

    public class MailService : IMailService
    {
        protected readonly SmtpClient client;
        public MailService()
        {
            var host = Environment.GetEnvironmentVariable("SEETRUE_SMTP_HOST");
            var port = int.Parse(Environment.GetEnvironmentVariable("SEETRUE_SMTP_PORT"));
            var user = Environment.GetEnvironmentVariable("SEETRUE_SMTP_USER");
            var pass = Environment.GetEnvironmentVariable("SEETRUE_SMTP_PASS");

            this.client = new SmtpClient("localhost", 1025);
            this.client.Credentials = new NetworkCredential(user, pass);
        }

        public async Task Send(string address, string code)
        {

            MailAddress from = new("service@mailhog.example", "Confirm user registration");
            MailAddress to = new(address);

            MailMessage message = new MailMessage(from, to)
            {
                Subject = "Passwordless Confirm",
                Body = $"<a href=\"http://localhost:5000/confirm/{code}\">confirm</a>",
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
    }
}

using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using SeeTrue.Models;
using SeeTrue.Infrastructure.Utils;
using SeeTrue.Infrastructure.Services;
using SeeTrue.Infrastructure.Types;
using Stubble.Core.Builders;
using Stubble.Core;

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
        private readonly IQueryService queries;
        protected readonly StubbleVisitorRenderer renderer;

        public MailService(IQueryService queries)
        {
            this.client = new SmtpClient(Env.SmtpHost, Env.SmtpPort)
            {
                Credentials = new NetworkCredential(Env.SmtpUser, Env.SmtpPass)
            };

            this.renderer =  new StubbleBuilder().Build();

            this.queries = queries;
        }

        public async Task NotifyConfirmation(User user)
        {

            MailAddress from = new("service@mailhog.example", "Confirm user registration");
            MailAddress to = new(user.Email);

            Mail mail = await queries.GetMailTemplate(NotificationType.Confirmation, user.Aud, user.Language);

            MailMessage message = new MailMessage(from, to)
            {
                Subject = renderer.Render(mail.Subject,user),
                Body = renderer.Render(mail.Content,user),
                IsBodyHtml = true
            };

            await Task.Run(() => client.Send(message));
        }

        public async Task NotifyEmailChange(User user, string email)
        {
            MailAddress from = new("service@mailhog.example", "Confirm email change");
            MailAddress to = new(user.Email);

            Mail mail = await queries.GetMailTemplate(NotificationType.EmailChange, user.Aud, user.Language);

            MailMessage message = new MailMessage(from, to)
            {
                Subject = renderer.Render(mail.Subject, user),
                Body = renderer.Render(mail.Content, user),
                IsBodyHtml = true
            };

            await Task.Run(() => client.Send(message));
        }

        public async Task NotifyRecovery(User user)
        {
            MailAddress from = new("service@mailhog.example", "Confirm email change");
            MailAddress to = new(user.Email);

            Mail mail = await queries.GetMailTemplate(NotificationType.Recovery, user.Aud, user.Language);

            MailMessage message = new MailMessage(from, to)
            {
                Subject = renderer.Render(mail.Subject, user),
                Body = renderer.Render(mail.Content, user),
                IsBodyHtml = true
            };

            await Task.Run(() => client.Send(message));
        }

        public async Task NotifyMagicLink(User user, string token)
        {
            MailAddress from = new("service@mailhog.example", "Confirm email change");
            MailAddress to = new(user.Email);

            Mail mail = await queries.GetMailTemplate(NotificationType.MagicLink, user.Aud, user.Language);

            MailMessage message = new MailMessage(from, to)
            {
                Subject = renderer.Render(mail.Subject, user),
                Body = renderer.Render(mail.Content, TypeMerger.TypeMerger.Merge(user,new { MagicToken = token })),
                IsBodyHtml = true
            };

            await Task.Run(() => client.Send(message));
        }

        public async Task NotifyInviteUser(User user)
        {
            MailAddress from = new("service@mailhog.example", "Confirm user invitation");
            MailAddress to = new(user.Email);

            Mail mail = await queries.GetMailTemplate(NotificationType.InviteUser, user.Aud, user.Language);

            MailMessage message = new MailMessage(from, to)
            {
                Subject = renderer.Render(mail.Subject, user),
                Body = renderer.Render(mail.Content, user),
                IsBodyHtml = true
            };

            await Task.Run(() => client.Send(message));
        }
    }
}

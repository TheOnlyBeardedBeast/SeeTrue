using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SeeTrue.Utils.Services
{
    public interface IMailService
    {
        Task Send(string address, string code);
    }

    // TODO: add dynamic config
    public class MailService : IMailService
    {
        protected readonly SmtpClient client;


        public MailService()
        {
            this.client = new SmtpClient("localhost", 1025);
        }

        public async Task Send(string address, string code)
        {

            MailAddress from = new("service@mailhog.example", "Paswordless");
            MailAddress to = new(address);

            MailMessage message = new MailMessage(from, to)
            {
                Subject = "Passwordless",
                Body = $"<a href=\"http://localhost:5000/confirm/{code}\">confirm</a>",
                IsBodyHtml = true
            };

            await Task.Run(() => client.Send(message));
        }
    }
}

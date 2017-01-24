using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendGrid;
using WebApplication.Contracts;


namespace WebApplication.Services {
    public class MailService : IMailService {
        private readonly SendGrid.Web _client;
        private readonly string _fromAddress;

        public MailService(IConfigurationRoot configuration) {
            _client = new SendGrid.Web(configuration["SENDGRID_APIKEY"]);
            _fromAddress = configuration["SENDGRID_FROM"];
        }

        public async Task SendMessage(string recipient, string subject, string body) {
            var fromAddress = new MailAddress(_fromAddress);

            var message = new SendGridMessage();
            message.From = fromAddress;
            message.AddTo(recipient);
            message.Subject = subject;
            message.Text = body;

            await _client.DeliverAsync(message);
        }
    }
}
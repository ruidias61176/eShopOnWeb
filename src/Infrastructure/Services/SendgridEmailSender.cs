using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Infrastructure.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class SendgridEmailSender : IEmailSender
    {
        private readonly ILogger<SendgridEmailSender> _logger;
        private readonly IConfiguration _configuration;

        public SendgridEmailSender(ILoggerFactory loggerFactory, IConfiguration configuration){
            _logger = loggerFactory.CreateLogger<SendgridEmailSender>();
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var apiKeyString = _configuration.GetValue<string>("SendGrid:apiKey");
            
            if(string.IsNullOrEmpty(apiKeyString)){
                throw new Exception("SendGrid apiKey is null or empty");
            }

            var from = _configuration.GetValue<string>("SendGrid:from");
            var fromName = _configuration.GetValue<string>("SendGrid:fromName");

            if(string.IsNullOrEmpty(from)){
                throw new Exception("Email From is null or empty");
            }

            var emailAddressFrom = new EmailAddress(from, fromName);
            var emailAddressTo   = new EmailAddress(email);
            string plainTextContent = "";
            SendGridMessage sendGridMessage = MailHelper.CreateSingleEmail(emailAddressFrom, emailAddressTo, subject, plainTextContent, message);

            var client = new SendGridClient(apiKeyString);
            var response = await client.SendEmailAsync(sendGridMessage);
            if(response.StatusCode == HttpStatusCode.Accepted){
                _logger.LogInformation($"Send e-mail to {email} is accepted.");
            }
            else{
                _logger.LogError($"Send e-mail to {email} is not accepted. {response.ToString()}");
                throw new Exception(response.ToString());
            }

        }
    }
}



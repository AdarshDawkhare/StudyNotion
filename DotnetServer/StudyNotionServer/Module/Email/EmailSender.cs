using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace StudyNotionServer.Module.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailSender> _logger;
        private readonly string? SenderEmailAddress = string.Empty;
        private readonly string? SenderEmailPassword = string.Empty;
        private readonly string? SMTPServerIP = string.Empty;
        private int SMTPServerPort = 0;
        public EmailSender(IConfiguration configuration,ILogger<EmailSender> logger) 
        {
            _configuration = configuration;
            _logger = logger;
            try
            {
                SenderEmailAddress = _configuration.GetValue<string>("EmailSettings:Email");
                SenderEmailPassword = _configuration.GetValue<string>("EmailSettings:Password");
                SMTPServerIP = _configuration.GetValue<string>("EmailSettings:IP");
                SMTPServerPort = _configuration.GetValue<int>("EmailSettings:PORT");

                if (string.IsNullOrEmpty(SenderEmailAddress) || string.IsNullOrEmpty(SenderEmailPassword) || string.IsNullOrEmpty(SMTPServerIP) || SMTPServerPort == 0)
                {
                    throw new ArgumentException("EmailSettings are not properly configured inside appsettings");
                }
            }
            catch(Exception ex) 
            {
                _logger.LogError($"Exception occured while loading EmailSettings in Email Module - {ex.ToString()}");
            }
        }

        public async Task<bool> SendEmailAsync(string email, string subject, string message)
        {
            bool result = false;
            try
            {
                using var client = new SmtpClient(SMTPServerIP, SMTPServerPort)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false, // IMPORTANT: must be false if you provide credentials
                    Credentials = new NetworkCredential(SenderEmailAddress, SenderEmailPassword)
                };

                await client.SendMailAsync(
                new MailMessage(from: SenderEmailAddress,
                                to: email,
                                subject,
                                message
                                ));

                _logger.LogInformation($"Email sent successfully to {email}");

                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while sending email to {email} - {ex.ToString()}");
                return false;
            }

            return result;
        }
    }
}

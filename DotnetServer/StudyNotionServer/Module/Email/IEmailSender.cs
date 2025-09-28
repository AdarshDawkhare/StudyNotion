namespace StudyNotionServer.Module.Email
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(string email , string subject , string message);
    }
}

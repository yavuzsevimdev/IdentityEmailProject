namespace MYAcademy_IdentityEmailProject.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string receiverEmail, string subject, string message);
        Task SendHtmlEmailAsync(string receiverEmail, string subject, string htmlMessage, string textMessage);


    }
}

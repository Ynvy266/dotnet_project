using System.Net.Mail;
using System.Net;

namespace dotnet_project.Areas.Admin.Repository
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true, //bật bảo mật
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("nguyenyenvy266@gmail.com", "pwyqlfwvcsnyxbrh")
            };

            return client.SendMailAsync(
                new MailMessage(from: "nguyenyenvy266@gmail.com",
                                to: email,
                                subject,
                                message
                                ));
        }
    }
}
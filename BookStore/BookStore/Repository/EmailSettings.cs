
using System.Net;
using System.Net.Mail;

namespace BookStore.Repository
{
    public class EmailSettings : IEmailSettings
    {
        public string fromMail { get; set; } = "tapp93550@gmail.com";
        public string fromPassword { get; set; } = "hijl zvbh ciyg sacu";




        public void sendEmail(string toMail, string subject, string body)
        {
            var Message = new MailMessage();
            Message.From = new MailAddress(fromMail);
            Message.To.Add(new MailAddress(toMail));
            Message.Subject = subject;
            Message.Body = body;
            Message.IsBodyHtml = false;
            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };
            smtp.Send(Message);

        }
    }
}

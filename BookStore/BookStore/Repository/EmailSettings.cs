
using System.Net;
using System.Net.Mail;

namespace BookStore.Repository
{
    public class EmailSettings : IEmailSettings
    {
        public string fromMail { get; set; } = "tapp93550@gmail.com";
        public string fromPassword { get; set; } = "hijl zvbh ciyg sacu";

        public string toMail { get; set; } = "bookhivea@gmail.com";


        public void sendEmail(string toMail, string subject, string body)
        {
            this.toMail = toMail;
            var Message = new MailMessage();
            Message.From = new MailAddress(fromMail);
            Message.To.Add(new MailAddress(this.toMail));
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

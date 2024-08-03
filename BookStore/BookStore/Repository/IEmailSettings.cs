using System.Globalization;

namespace BookStore.Repository
{
    public interface IEmailSettings
    {
        string fromMail{ get; set; }
    string fromPassword {  get; set; }
       
        public void sendEmail(string toMail,string subject, string body);
    }
}

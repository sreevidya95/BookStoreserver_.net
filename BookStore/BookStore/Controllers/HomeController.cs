using BookStore.Repository;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class SaleController : Controller
    {
        private readonly IEmailSettings email;

        public SaleController(IEmailSettings email)
        {
            this.email = email;
        }
        [HttpPost]
       public IActionResult SetReminder (Models.Reminder reminder)
        {
            try
            {
                var selectedDate = DateTimeOffset.Parse(reminder.date.ToString());
                BackgroundJob.Schedule(() => email.sendEmail(reminder.email, "Reminder", reminder.event_name), selectedDate);
               
                return Ok("Scheduled");
            }
            catch (Exception ex) { 
                  return Ok(ex.ToString());
            }
        }
        
    }
}

namespace BookStore.Models
{
    /// <summary>
    /// To set Reminder
    /// </summary>
    public class Reminder
    {
        /// <summary>
        /// Email Id
        /// </summary>
        public string? email { get; set; } = "bookhivea@gmail.com";
        /// <summary>
        /// Email Body
        /// </summary>
        public string? event_name { get; set; } = null;
        /// <summary>
        /// Date on which You want to set reminder for
        /// </summary>
        public DateTime? date {  get; set; }=DateTime.Now;

    }
}

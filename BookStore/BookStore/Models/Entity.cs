using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    /// <summary>
    /// Enquiry Details
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// Enquiry Id
        /// </summary>
        public int enq_id { get; set; }
        /// <summary>
        /// Message from User
        /// </summary>
        public string? message { get; set; }
       /// <summary>
       /// User's email
       /// </summary>
        public string? user_email { get; set; }
        /// <summary>
        /// Read by admin or not
        /// </summary>
        public byte isRead { get; set; } = 0;
    }
}

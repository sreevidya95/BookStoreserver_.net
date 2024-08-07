using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    /// <summary>
    /// Admin Details
    /// </summary>
    public class Admin
    {
        /// <summary>
        /// Admin Id
        /// </summary>
        public int admin_id { get; set; }
        /// <summary>
        /// Admin's Name
        /// </summary>
        public string? name { get; set; }
        /// <summary>
        /// Admin's Email
        /// </summary>
        public string? email { get; set; }
        /// <summary>
        /// Admin's Password
        /// </summary>
        public string? password { get; set; }
        /// <summary>
        /// Authenication Token
        /// </summary>
        public string? authenticationToken { get; set; }=string.Empty;
    }
}

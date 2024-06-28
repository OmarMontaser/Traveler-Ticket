using System.ComponentModel.DataAnnotations;

namespace TravellerTicket.Core.Context
{
    public class LogInModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

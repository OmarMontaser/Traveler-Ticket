using System.ComponentModel.DataAnnotations;

namespace TravellerTicket.Core.DTO.Admin
{
    public class CreateTicketAdminDTO
    {
        [Required]
        public DateTime Time { get; set; }

        [Required]
        [StringLength(100)]
        public string From { get; set; }

        [Required]
        [StringLength(100)]
        public string To { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Price { get; set; }
        public string PassengerName { get; set; }

        public string PassengerId { get; set; }

    }
}

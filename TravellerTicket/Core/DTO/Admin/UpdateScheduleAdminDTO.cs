using System.ComponentModel.DataAnnotations;

namespace TravellerTicket.Core.DTO.Admin
{
    public class UpdateScheduleAdminDTO
    {
        [Required]
        public string? From { get; set; }

        [Required]
        public string? To { get; set; }

        [Required]
        public DateTime? Time { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double? Price { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int? Capacity { get; set; }
    }
}

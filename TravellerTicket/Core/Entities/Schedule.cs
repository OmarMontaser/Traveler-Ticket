using System.ComponentModel.DataAnnotations;

namespace TravellerTicket.Core.Entities
{
    public class Schedule
    {
        [Key]
        public int ScheduleId { get; set; }
        public DateTime Time { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public double Price { get; set; }
        public int Capacity { get; set; }
        // public string AdminId { get; set; }  // From JWT
    }
}

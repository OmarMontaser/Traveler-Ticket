using System.ComponentModel.DataAnnotations;

namespace TravellerTicket.Core.Entities
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        public DateTime Time { get; set; }
        public string PassengerName { get; set; }
        public string PassengerId { get; set; }  // From JWT   DID NOT WORK 
        public string From { get; set; }
        public string To { get; set; }
        public double Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public string ConfidentialComment { get; set; } = "Normal";

        // Foreign Key to Schedule
        //public int ScheduleId { get; set; }
        //public Schedule Schedule { get; set; }
    }
}

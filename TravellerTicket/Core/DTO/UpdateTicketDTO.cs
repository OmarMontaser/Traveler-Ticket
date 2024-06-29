using System.ComponentModel.DataAnnotations;

namespace TravellerTicket.Core.DTO
{
    public class UpdateTicketDTO
    {
        public DateTime Time { get; set; }
        public string PassengerName { get; set; }
        public int PassengerId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public double Price { get; set; }
    }
}

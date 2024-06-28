namespace TravellerTicket.Core.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string PassengerName { get; set; }
        public int PassengerId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public double Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public string ConfidentialComment { get; set; } = "Normal";
    }
}

namespace TravellerTicket.Core.DTO
{
    public class CreateTicketDTO
    {
        public DateTime Time {  get; set; }
        public string PassengerName { get; set; }
        public string PassangerSSN { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Price { get; set; }

    }
}

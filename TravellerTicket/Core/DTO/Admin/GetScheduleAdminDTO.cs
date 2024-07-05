namespace TravellerTicket.Core.DTO.Admin
{
    public class GetScheduleAdminDTO
    {
        public int ScheduleId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime Time { get; set; }
        public double Price { get; set; }
        public int Capacity { get; set; }
    }
}

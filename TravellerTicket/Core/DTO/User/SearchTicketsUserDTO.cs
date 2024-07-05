namespace TravellerTicket.Core.DTO.User
{
    public class SearchTicketsUserDTO
    {
            public DateTime? Date { get; set; }
            public string? From { get; set; }
            public string? To { get; set; }
            public string? TravelClass { get; set; }   // == ConfidentialComment

    }
}

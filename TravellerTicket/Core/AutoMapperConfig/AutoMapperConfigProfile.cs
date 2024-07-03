using AutoMapper;
using TravellerTicket.Core.DTO;
using TravellerTicket.Core.Entities;

namespace TravellerTicket.Core.AutoMapperConfig
{
    public class AutoMapperConfigProfile : Profile
    {
        public AutoMapperConfigProfile() 
        {
            CreateMap<CreateTicketDTO, Ticket>();
            CreateMap<Ticket, GetTicketDTO>();
            CreateMap<UpdateTicketDTO,Ticket>();
           
            CreateMap<CreateScheduleDTO, Schedule>();
            CreateMap<Schedule, GetScheduleDTO>();
            CreateMap<UpdateScheduleDTO, Schedule>();
        }

    }
}

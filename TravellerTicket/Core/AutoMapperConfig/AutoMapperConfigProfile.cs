using AutoMapper;
using TravellerTicket.Core.Context;
using TravellerTicket.Core.DTO.Admin;
using TravellerTicket.Core.DTO.User;
using TravellerTicket.Core.Entities;

namespace TravellerTicket.Core.AutoMapperConfig
{
    public class AutoMapperConfigProfile : Profile
    {
        public AutoMapperConfigProfile() 
        {
            CreateMap<CreateTicketAdminDTO, Ticket>();
            CreateMap<Ticket, GetTicketAdminDTO>();
            CreateMap<UpdateTicketAdminDTO,Ticket>();
           
            CreateMap<CreateScheduleAdminDTO, Schedule>();
            CreateMap<Schedule, GetScheduleAdminDTO>();
            CreateMap<UpdateScheduleAdminDTO, Schedule>();


            CreateMap<Ticket, GetTicketUserDTO>();



            CreateMap<ApplicationUser, UserDTO>();
            CreateMap<CreateUserDTO, ApplicationUser>();

        }

    }
}

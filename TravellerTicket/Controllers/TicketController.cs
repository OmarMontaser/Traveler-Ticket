using Microsoft.AspNetCore.Mvc;
using TravellerTicket.Core.Context;

namespace TravellerTicket.Controllers
{
    [Route("Api/[Controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public TicketController(ApplicationDbContext context)
        {
            _context = context;
        }
         


    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravellerTicket.Core.Context;
using TravellerTicket.Core.DTO;
using TravellerTicket.Core.Entities;

namespace TravellerTicket.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public TicketController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Add Ticket
        [HttpPost("addticket")]
        public async Task<IActionResult> AddTicket([FromBody] CreateTicketDTO addticket)
        {
            var newTicket = new Ticket();

            _mapper.Map(addticket, newTicket);
            await _context.Tickets.AddAsync(newTicket);
            await _context.SaveChangesAsync();

            return Ok(newTicket);

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetTicketDTO>>> GetTickets()
        {
            var tickets = await _context.Tickets.ToListAsync();

            var convertedTickets = _mapper.Map<IEnumerable<GetTicketDTO>>(tickets);

            return Ok(convertedTickets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetTicketDTO>> GetTicketById([FromRoute] int id)
        {

            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
            if (ticket is null)
            {
                return NotFound("Ticket Not Found");
            }

            var convertedTicket = _mapper.Map<GetTicketDTO>(ticket);
            return Ok(convertedTicket);
        }

        [HttpPut("editTicket/{id}")]
        public async Task<IActionResult> EditTicket([FromRoute] int id,[FromBody] UpdateTicketDTO updateTicketDTO)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
            if(ticket is null)
            {
                return BadRequest("Ticket Not Found");
            }
            _mapper.Map(updateTicketDTO, ticket);
            ticket.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(ticket);
        }

        [HttpDelete("DeleteTicket/{id}")]
        public async Task<IActionResult> DeleteTicket([FromRoute] int id)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
            if( ticket is null)
            {
                return BadRequest("Ticket Not Found");
            }
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return Ok("Ticket Deleted Succefully");
        }


    }
}

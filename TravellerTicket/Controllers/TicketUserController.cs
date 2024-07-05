using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TravellerTicket.Core.Context;
using TravellerTicket.Core.DTO.User;
using TravellerTicket.Core.Entities;

namespace TravellerTicket.Controllers
{
    //[Authorize(Roles = "User")]
    [Route("api/[Controller]")]
    [ApiController]
    public class TicketUserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TicketUserController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<GetTicketUserDTO>>> SearchTickets([FromQuery] SearchTicketsUserDTO searchCriteria)
        {
            IQueryable<Ticket> query = _context.Tickets;
            if (searchCriteria.Date.HasValue)
            {
                query = query.Where(t => t.Time.Date == searchCriteria.Date.Value.Date);
            }
            if (!string.IsNullOrEmpty(searchCriteria.From))
            {
                query = query.Where(t => t.From.Contains(searchCriteria.From));
            }
            if (!string.IsNullOrEmpty(searchCriteria.To))
            {
                query = query.Where(t => t.To.Contains(searchCriteria.To));
            }
            if (!string.IsNullOrEmpty(searchCriteria.TravelClass))
            {
                query = query.Where(t => t.ConfidentialComment.Contains(searchCriteria.TravelClass));
            }

            var tickets = await query.ToListAsync();
            var convertedTickets = _mapper.Map<IEnumerable<GetTicketUserDTO>>(tickets);
            return Ok(convertedTickets);
        }


        [HttpPost("book")]
        public async Task<IActionResult> BookTicket([FromBody] BookTicketUserDTO bookTicketDTO)
        {
            var schedule = await _context.Schedules.FindAsync(bookTicketDTO.ScheduleId);
            if (schedule == null)
            {
                return NotFound("Schedule not found");
            }

            var newTicket = new Ticket
            {
                //ScheduleId = bookTicketDTO.ScheduleId,
                PassengerId = bookTicketDTO.PassengerId,
                PassengerName = bookTicketDTO.PassengerName,
                From = schedule.From,
                To = schedule.To,
                Time = schedule.Time,
                Price = schedule.Price
            };

            await _context.Tickets.AddAsync(newTicket);
            await _context.SaveChangesAsync();
            return Ok(newTicket);

        }


        [HttpGet("bookings/{userId}")]
        public async Task<ActionResult<IEnumerable<GetTicketUserDTO>>> GetUserBookings([FromRoute]string userId)
        {
            var tickets = await _context.Tickets
                .Where(t => t.PassengerId == userId)
                .ToListAsync();
            
            if(tickets.IsNullOrEmpty())
            {
                return BadRequest("Booking ticket Not Found");
            }

            var convertedTickets = _mapper.Map<IEnumerable<GetTicketUserDTO>>(tickets);
            return Ok(convertedTickets);
        }


        [HttpDelete("cancel/{ticketId}")]
        public async Task<IActionResult> CancelTicket(int ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null)
            {
                return NotFound("Ticket not found");
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            // Initiate refund process here if needed

            return Ok("Ticket cancelled and refund initiated");
        }


        [HttpPut("reschedule")]
        public async Task<IActionResult> RescheduleTicket([FromBody] RescheduleTicketUserDTO rescheduleTicketDTO)
        {
            var ticket = await _context.Tickets.FindAsync(rescheduleTicketDTO.TicketId);
            if (ticket == null)
            {
                return NotFound("Ticket not found");
            }

            ticket.Time = rescheduleTicketDTO.NewDate;
            ticket.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok(ticket);
        }


    }
}
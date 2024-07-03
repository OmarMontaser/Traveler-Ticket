using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravellerTicket.Core.Context;
using TravellerTicket.Core.DTO;
using TravellerTicket.Core.Entities;
using TravellerTicket.Helper;

namespace TravellerTicket.Controllers
{
    //[Authorize(Roles = "User")]
    [Route("api/[Controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public TicketController(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _mapper = mapper;
            _roleManager = roleManager;
            _userManager = userManager;
        }



        [HttpPost("addticket")]
        public async Task<IActionResult> AddTicket([FromBody] CreateTicketDTO addTicket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<string> { Success = false, Message = "Invalid data", Data = null });
            }

            var newTicket = _mapper.Map<Ticket>(addTicket);

            await _context.Tickets.AddAsync(newTicket);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<Ticket> { Success = true, Message = "Ticket added successfully", Data = newTicket });
        }



        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<GetTicketDTO>>>> GetTickets([FromQuery] PaginationParams param, [FromQuery] string? q)
        {
            IQueryable<Ticket> query = _context.Tickets;
            if (q is not null)
            {
                query = query.Where(t => t.PassengerName.Contains(q));
            }

            var tickets = await query.Skip((param.PageNumber - 1) * param.PageSize)
                             .Take(param.PageSize)
                             .ToListAsync();


            var convertedTickets = _mapper.Map<IEnumerable<GetTicketDTO>>(tickets);

            return Ok(new ApiResponse<IEnumerable<GetTicketDTO>>
            {
                Success = true,
                Message = "Tickets retrieved successfully",
                Data = convertedTickets
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetTicketDTO>> GetTicketById([FromRoute] int id)
        {

            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.TicketId == id);
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
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.TicketId == id);
            if(ticket is null)
            {
                return BadRequest("Ticket Not Found");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(updateTicketDTO, ticket);
            ticket.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok(ticket);
        }


        [HttpDelete("DeleteTicket/{id}")]
        public async Task<IActionResult> DeleteTicket([FromRoute] int id)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.TicketId == id);
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

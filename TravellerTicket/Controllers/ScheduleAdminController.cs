﻿using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravellerTicket.Core.Context;
using TravellerTicket.Core.Entities;
using TravellerTicket.Helper;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using TravellerTicket.Core.DTO.Admin;

namespace TravellerTicket.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[Controller]")]
    [ApiController]
    public class ScheduleAdminController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ScheduleAdminController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Add Schedule
        [HttpPost("addSchedule")]
        public async Task<IActionResult> AddSchedule([FromBody] CreateScheduleAdminDTO createScheduleDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var newSchedule = _mapper.Map<Schedule>(createScheduleDTO);

            await _context.Schedules.AddAsync(newSchedule);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<Schedule> { Success = true, Message = "Schedule added successfully", Data = newSchedule });
        }

        // Get all Schedules
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<GetScheduleAdminDTO>>>> GetSchedules([FromQuery] PaginationParams param, [FromQuery] string? q)
        {
            IQueryable<Schedule> query = _context.Schedules;
            if (q is not null)
            {
                query = query.Where(s => s.Capacity.ToString().Contains(q));
            }

            var schedules = await query.Skip((param.PageNumber - 1) * param.PageSize)
                             .Take(param.PageSize)
                             .ToListAsync();

            var scheduleDTOs = _mapper.Map<IEnumerable<GetScheduleAdminDTO>>(schedules);

            return Ok(new ApiResponse<IEnumerable<GetScheduleAdminDTO>>
            { 
              Success = true,
              Message = "Schedules retrieved successfully",
              Data = scheduleDTOs
            });
        }

        // Get Schedule by Id
        [HttpGet("schedule/{id}")]
        public async Task<ActionResult<ApiResponse<GetScheduleAdminDTO>>> GetScheduleById(int id)
        {
            var schedule = await _context.Schedules.FirstOrDefaultAsync(s => s.ScheduleId == id);
            if (schedule == null)
            {
                return NotFound(new ApiResponse<string> { Success = false, Message = "Schedule not found", Data = null });
            }

            var scheduleDTO = _mapper.Map<GetScheduleAdminDTO>(schedule);
            return Ok(new ApiResponse<GetScheduleAdminDTO> { Success = true, Message = "Schedule retrieved successfully", Data = scheduleDTO });
        }

        // Update Schedule
        [HttpPut("editSchedule/{id}")]
        public async Task<IActionResult> EditSchedule(int id, [FromBody] UpdateScheduleAdminDTO updateScheduleDTO)
        {
            var schedule = await _context.Schedules.FirstOrDefaultAsync(s => s.ScheduleId == id);
            if (schedule == null)
            {
                return BadRequest(new ApiResponse<string> { Success = false, Message = "Schedule not found", Data = null });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(updateScheduleDTO, schedule);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<Schedule> { Success = true, Message = "Schedule updated successfully", Data = schedule });
        }

        // Delete Schedule
        [HttpDelete("deleteSchedule/{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var schedule = await _context.Schedules.FirstOrDefaultAsync(s => s.ScheduleId == id);
            if (schedule == null)
            {
                return BadRequest(new ApiResponse<string> { Success = false, Message = "Schedule not found", Data = null });
            }

            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return Ok(new ApiResponse<string> { Success = true, Message = "Schedule deleted successfully", Data = null });
        }



    }
}

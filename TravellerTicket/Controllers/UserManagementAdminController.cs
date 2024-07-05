using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravellerTicket.Core.Context;
using TravellerTicket.Core.DTO.Admin;
using TravellerTicket.Core.Entities;
using TravellerTicket.Helper;

namespace TravellerTicket.Controllers
{

        [Route("api/Admin/[controller]")]
        [ApiController]
        public class UserManagementController : ControllerBase
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly IMapper  _mapper ; 

            public UserManagementController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper  mapper)
            {
                _userManager = userManager;
                _roleManager = roleManager;
                _mapper = mapper;
            }

        [HttpGet("users")]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserDTO>>>> GetUsers([FromQuery] PaginationParams param, [FromQuery] string? q)
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(u => u.FirstName.Contains(q) || u.LastName.Contains(q) || u.UserName.Contains(q));
            }

            var users = await query.Skip((param.PageNumber - 1) * param.PageSize)
                                   .Take(param.PageSize)
                                   .ToListAsync();

            var userDTOs = _mapper.Map<IEnumerable<UserDTO>>(users);

            return Ok(new ApiResponse<IEnumerable<UserDTO>>
            {
                Success = true,
                Message = "Users retrieved successfully",
                Data = userDTOs
            });
        }

   
            [HttpPost("create")]
            public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO model)
            {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user =  _mapper.Map<ApplicationUser>(model);

                var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "User creation failed",
                    Data = result.Errors.Select(e => e.Description)
                });
            }

            if (model.Roles != null && model.Roles.Any())
            {
                var roleResult = await _userManager.AddToRolesAsync(user, model.Roles);
                if (!roleResult.Succeeded)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Failed to assign roles",
                        Data = roleResult.Errors.Select(e => e.Description)
                    });
                }
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var userDTO = _mapper.Map<UserDTO>(user);
            userDTO.Roles = userRoles.ToList();

            return Ok(new ApiResponse<UserDTO>
            {
                Success = true,
                Message = "User created successfully",
                Data = userDTO
            });

        }

            [HttpPut("update")]
            public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO model)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.UserName = model.UserName;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, userRoles.ToArray());
                    await _userManager.AddToRolesAsync(user, model.Roles);
                    return Ok(user);
                }

                return BadRequest(result.Errors);
            }

            [HttpDelete("delete/{id}")]
            public async Task<IActionResult> DeleteUser(string id)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return Ok("User deleted successfully");
                }

                return BadRequest(result.Errors);
            }

            [HttpGet("roles")]
            public async Task<IActionResult> GetRoles()
            {
                var roles = _roleManager.Roles.Select(role => new RoleDTO
                {
                    Id = role.Id,
                    Name = role.Name
                }).ToList();

                return Ok(roles);
            }

            [HttpPost("roles")]
            public async Task<IActionResult> CreateRole([FromBody] RoleDTO model)
            {
                var role = new IdentityRole { Name = model.Name };

                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return Ok(role);
                }

                return BadRequest(result.Errors);
            }

            [HttpDelete("roles/{id}")]
            public async Task<IActionResult> DeleteRole(string id)
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return NotFound("Role not found");
                }

                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return Ok("Role deleted successfully");
                }

                return BadRequest(result.Errors);
            }
        }
}
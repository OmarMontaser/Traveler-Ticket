using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravellerTicket.Core.Context;
using TravellerTicket.Services;

namespace TravellerTicket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
             private readonly IAuthService _authService;
             private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(IAuthService authService,
            SignInManager<ApplicationUser> signInManager)
            {
                _authService = authService;
                _signInManager = signInManager;
            }

            [HttpPost("register")]
            public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _authService.RegisterAsync(model);

                if (!result.IsAuthenticated)
                    return BadRequest(result.Message);


                return Ok(result);

            }

            [HttpPost("login")]
            public async Task<IActionResult> LoginAsync([FromBody] LogInModel model)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _authService.GetTokenAsync(model);

                if (!result.IsAuthenticated)
                    return BadRequest(result.Message);


                return Ok(result);

            }

            [HttpPost("addrole")]
            public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _authService.AddRoleAsync(model);

                if (!string.IsNullOrEmpty(result))
                    return BadRequest(result);


                return Ok(model);

            }
    }
}
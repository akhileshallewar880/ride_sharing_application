using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RideSharing.API.CustomValidations;
using RideSharing.API.Models.DTO;
using RideSharing.API.Repositories.Interface;

namespace RideSharing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        // POST: api/Auth/register
        [HttpPost("register")]
        [ValidateModel]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {

            var user = new IdentityUser
            {
                UserName = registerUserDto.Email,
                Email = registerUserDto.Email,
            };

            var result = await _userManager.CreateAsync(user, registerUserDto.Password);
            if (!result.Succeeded)
            {
                // Return detailed validation errors instead of a generic message
                return BadRequest(new
                {
                    Errors = result.Errors.Select(e => e.Description)
                });
            }

            var roleResult = await _userManager.AddToRoleAsync(user, registerUserDto.Role);
            if (!roleResult.Succeeded)
            {
                // If adding role fails, delete the created user to keep state clean (best-effort)
                await _userManager.DeleteAsync(user);
                return BadRequest(new
                {
                    Errors = roleResult.Errors.Select(e => e.Description)
                });
            }

            // Avoid CreatedAtAction because there is no GET endpoint to resolve; return a simple success payload
            return Ok(new { Id = user.Id, Email = user.Email, Role = registerUserDto.Role });
        }

        [HttpPost("login")]
        [ValidateModel]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginUserDto)
        {
            var user = await _userManager.FindByNameAsync(loginUserDto.UserName);
            if (user == null)
            {
                return BadRequest("Invalid username or password");
            }

            var result = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);
            if (!result)
            {
                return BadRequest("Invalid email or password");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenRepository.CreateJwtToken(user, roles.ToList());

            return Ok(new { Token = token });
        }
    }
}

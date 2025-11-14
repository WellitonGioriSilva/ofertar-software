using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_ofertar.DTOs;
using api_ofertar.DTOs.Responses;
using api_ofertar.Entities;
using api_ofertar.Responses;
using api_ofertar.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace api_ofertar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "ActiveChurch")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        private int? GetChurchIdFromJwt()
        {
            var churchIdClaim = User.FindFirst("churchId");
            if (churchIdClaim != null && int.TryParse(churchIdClaim.Value, out var churchId))
            {
                return churchId;
            }
            return null;
        }

        // Routes
        
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<List<UserResponseDTO>>>> GetUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync(null);
                return Ok(ApiResponse<List<UserResponseDTO>>.Ok(users, take: users.Count, offset: 0, total: users.Count));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserResponseDTO>>> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                return Ok(ApiResponse<UserResponseDTO>.Ok(user));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse<UserResponseDTO>>> CreateUser([FromBody] UserCreateDTO userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<object>.Fail("Invalid user data"));

                int? churchId = null;

                // If churchId is not provided in DTO, try to get it from JWT
                if (!userDto.ChurchId.HasValue)
                {
                    churchId = GetChurchIdFromJwt();
                }
                else
                {
                    churchId = userDto.ChurchId;
                }

                var result = await _userService.CreateUserAsync(userDto, userDto.Roles ?? new List<UserRoleCreateDTO>(), churchId);
                return CreatedAtAction(nameof(GetUserById), new { id = result.Id }, ApiResponse<UserResponseDTO>.Ok(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<UserResponseDTO>>> UpdateUser(int id, [FromBody] UserUpdateDTO userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<object>.Fail("Invalid user data"));

                int? churchId = null;

                // If churchId is not provided in DTO, try to get it from JWT
                if (!userDto.ChurchId.HasValue)
                {
                    churchId = GetChurchIdFromJwt();
                }
                else
                {
                    churchId = userDto.ChurchId;
                }

                var result = await _userService.UpdateUserAsync(id, userDto, userDto.Roles ?? new List<UserRoleCreateDTO>(), churchId);
                return Ok(ApiResponse<UserResponseDTO>.Ok(result));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<string>>> Login([FromBody] UserLoginDTO loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<object>.Fail("Invalid login data"));

                var token = await _userService.LoginUserAsync(loginDto.Email, loginDto.Password);
                return Ok(ApiResponse<string>.Ok(token, "Login successful."));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<string>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }

        [HttpPost("recover-password")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<object>>> RecoverPassword([FromBody] UserPasswordRecoveryDTO recoveryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<object>.Fail("Invalid recovery data"));

                await _userService.SendRecoveryTokenAsync(recoveryDto);
                return Ok(ApiResponse<object>.Ok(null, "Recovery token sent successfully."));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<object>>> ResetPassword([FromBody] UserPasswordResetDTO resetDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ApiResponse<object>.Fail("Invalid reset data"));

                await _userService.ResetPasswordAsync(resetDto);
                return Ok(ApiResponse<object>.Ok(null, "Password reset successfully."));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<object>.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }
    }
}
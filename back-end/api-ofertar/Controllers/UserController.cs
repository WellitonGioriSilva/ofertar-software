using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_ofertar.DTOs;
using api_ofertar.Entities;
using api_ofertar.Responses;
using api_ofertar.Services;
using Microsoft.AspNetCore.Mvc;

namespace api_ofertar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync(null);
            return Ok(ApiResponse<List<User>>.Ok(users));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = new User { Id = id, Name = "Alice", Email = "alice@example.com" };
            return Ok(ApiResponse<User>.Ok(user));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDTO userDto)
        {
            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
            };

            await _userService.CreateUserAsync(user, userDto.Roles);
            return Ok(ApiResponse<User>.Ok(user, "User created successfully."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDTO userDto)
        {
            var user = new User
            {
                Id = id,
                Name = userDto.Name,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
            };
            return Ok(ApiResponse<User>.Ok(user, "User updated successfully."));
        }
    }
}
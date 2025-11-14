using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_ofertar.Data;
using api_ofertar.DTOs;
using api_ofertar.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace api_ofertar.Services
{
    public class UserService
    {
        private readonly DataBaseConfig _dbContext;
        public UserService(DataBaseConfig dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<User>> GetAllUsersAsync(string? filter = null)
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task CreateUserAsync(User user, List<UserRoleCreateDTO> roles)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            if (roles != null && roles.Any())
            {
                var userRoles = roles.Select(r => new UserRole
                {
                    UserId = user.Id,
                    RoleId = r.RoleId
                }).ToList();
                _dbContext.UserRoles.AddRange(userRoles);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
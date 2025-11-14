using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_ofertar.Data;
using api_ofertar.DTOs;
using api_ofertar.DTOs.Responses;
using api_ofertar.Entities;
using api_ofertar.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace api_ofertar.Services
{
    public class UserService
    {
        private readonly DataBaseConfig _dbContext;
        private readonly JwtHelper _jwtHelper;
        private readonly EmailHelper _emailHelper;

        public UserService(DataBaseConfig dbContext, JwtHelper jwtHelper, EmailHelper emailHelper)
        {
            _dbContext = dbContext;
            _jwtHelper = jwtHelper;
            _emailHelper = emailHelper;
        }

        public async Task<List<UserResponseDTO>> GetAllUsersAsync(string? filter = null)
        {
            var users = await _dbContext.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ToListAsync();

            return users.Select(u => new UserResponseDTO
            {
                Id = u.Id,
                Email = u.Email,
                Name = u.Name,
                RecoveryToken = u.RecoveryToken,
                Roles = u.UserRoles.Select(ur => new UserRoleResponseDTO
                {
                    Id = ur.Id,
                    RoleId = ur.RoleId,
                    RoleName = ur.Role?.Name ?? String.Empty
                }).ToList()
            }).ToList();
        }

        public async Task<UserResponseDTO> CreateUserAsync(UserCreateDTO userDto, List<UserRoleCreateDTO> roles, int? churchId = null)
        {
            // Use a transaction to ensure user and roles are saved atomically
            await using var tx = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var user = new User
                {
                    Name = userDto.Name,
                    Email = userDto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                    ChurchId = churchId ?? userDto.ChurchId
                };

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

                await tx.CommitAsync();

                // Reload user to ensure navigation properties are populated
                var saved = await _dbContext.Users
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.Id == user.Id);

                if (saved == null)
                    throw new InvalidOperationException("User was not persisted to the database.");

                return new UserResponseDTO
                {
                    Id = saved.Id,
                    Email = saved.Email,
                    Name = saved.Name,
                    RecoveryToken = saved.RecoveryToken,
                    Roles = saved.UserRoles.Select(ur => new UserRoleResponseDTO
                    {
                        Id = ur.Id,
                        RoleId = ur.RoleId,
                        RoleName = ur.Role?.Name ?? String.Empty
                    }).ToList()
                };
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task<UserResponseDTO> UpdateUserAsync(int id, UserUpdateDTO userDto, List<UserRoleCreateDTO> roles, int? churchId = null)
        {
            var user = await _dbContext.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw new KeyNotFoundException($"User with id {id} not found.");

            user.Name = userDto.Name;
            user.Email = userDto.Email;
            if (!string.IsNullOrEmpty(userDto.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            user.ChurchId = churchId ?? userDto.ChurchId;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            var existingRoles = _dbContext.UserRoles.Where(ur => ur.UserId == user.Id).ToList();
            _dbContext.UserRoles.RemoveRange(existingRoles);
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

            return await GetUserByIdAsync(id);
        }

        public async Task<UserResponseDTO> GetUserByIdAsync(int id)
        {
            var user = await _dbContext.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw new KeyNotFoundException($"User with id {id} not found.");

            return new UserResponseDTO
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                RecoveryToken = user.RecoveryToken,
                Roles = user.UserRoles.Select(ur => new UserRoleResponseDTO
                {
                    Id = ur.Id,
                    RoleId = ur.RoleId,
                    RoleName = ur.Role?.Name ?? String.Empty
                }).ToList()
            };
        }

        public async Task<string> LoginUserAsync(string email, string password)
        {
            var user = await _dbContext.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            var roles = user.UserRoles.Select(ur => ur.Role?.Name ?? "").ToList();
            return _jwtHelper.GenerateJwtToken(user.Id, user.Email, roles, user.ChurchId);
        }

        public async Task SendRecoveryTokenAsync(UserPasswordRecoveryDTO recoveryDTO)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == recoveryDTO.Email);
            if (user == null)
                throw new KeyNotFoundException($"User with email {recoveryDTO.Email} not found.");

            user.RecoveryToken = _jwtHelper.GenerateJwtToken(user.Id, user.Email, null, null);
            await _dbContext.SaveChangesAsync();

            _emailHelper.SendEmailRecovery(user.Email, user.RecoveryToken);
        }

        public async Task ResetPasswordAsync(UserPasswordResetDTO resetDTO)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.RecoveryToken == resetDTO.ResetToken);
            if (user == null)
                throw new KeyNotFoundException("Invalid recovery token.");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetDTO.NewPassword);
            user.RecoveryToken = null;
            await _dbContext.SaveChangesAsync();
        }
    }
}
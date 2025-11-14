using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_ofertar.DTOs.Responses
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Email { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public string RecoveryToken { get; set; } = String.Empty;
        public List<UserRoleResponseDTO>? Roles { get; set; }
    }

    public class UserRoleResponseDTO
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; } = String.Empty;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api_ofertar.DTOs
{
    public class UserRoleCreateDTO
    {

        [Required(ErrorMessage = "RoleId is required.")]
        public int RoleId { get; set; }
    }

    public class UserRoleUpdateDTO
    {
        public int? RoleId { get; set; }
    }
}
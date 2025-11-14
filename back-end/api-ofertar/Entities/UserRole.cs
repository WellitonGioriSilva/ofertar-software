using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api_ofertar.Entities
{
    [Table("UserRole")]
    public class UserRole
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")] 
        public int UserId { get; set; }
        public User? User { get; set; }
        
        [Column("role_id")] 
        public int RoleId { get; set; }
        public Role? Role { get; set; }
    }
}

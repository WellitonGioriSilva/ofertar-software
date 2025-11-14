using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api_ofertar.Entities
{
    [Table("Role")]
    public class Role
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
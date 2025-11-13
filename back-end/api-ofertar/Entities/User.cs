using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api_ofertar.Entities
{
    [Table("User")]
    public class User
    {
        [Column("id")]
        public int Id {get; set;}

        [Column("email")]
        public string Email {get; set;} = String.Empty;

        [Column("passwordHash")]
        public string PasswordHash {get; set;} = String.Empty;

        [Column("name")]
        public string Name {get; set;} = String.Empty;

        [Column("recoveryToken")]
        public string RecoveryToken {get; set;} = String.Empty;
        public ICollection<Tithe> Tithes { get; set; } = new List<Tithe>();
    }
}
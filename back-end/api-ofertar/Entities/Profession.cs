using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api_ofertar.Entities
{
    [Table("Profession")]
    public class Profession
    {
        [Column("id")]
        public int Id {get; set;}

        [Column("code")]
        public string Code {get; set;} = String.Empty;

        [Column("name")]
        public string Name {get; set;} = String.Empty;

        public ICollection<Tither> Tithers { get; set; } = new List<Tither>();
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api_ofertar.Entities
{
    [Table("Address")]
    public class Address
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("street")]
        public string Street {get; set;} = String.Empty;

        [Column("number")]
        public string Number {get; set;} = String.Empty;

        [Column("zipCode")]
        public string ZipCode {get; set;} = String.Empty;

        [Column("complement")]
        public string Complement {get; set;} = String.Empty;

        [Column("neighborhood")]
        public string Neighborhood {get; set;} = String.Empty;

    }
}
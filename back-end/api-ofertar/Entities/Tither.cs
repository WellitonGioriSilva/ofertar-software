using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using api_ofertar.Enums;

namespace api_ofertar.Entities
{
    [Table("Tither")]
    public class Tither
    {
        [Column("id")]
        public int Id {get; set;}

        [Column("phone")]
        public string Phone {get; set;} = String.Empty;

        [Column("email")]
        public string email {get; set;} = String.Empty;

        [Column("birthDate")]
        public DateTime BirthDate {get; set;}

        [Column("maritalStatus")]
        public MaritalStatus MaritalStatus { get; set; }

        [Column("isActive")]
        public bool IsActive {get; set;}

        [Column("company")]
        public string Company {get; set;} = String.Empty;

        [Column("name")]
        public string Name {get; set;} = String.Empty;

        [Column("spouse_id")]
        public int SpouseId {get; set;}
        public Tither? Spouse {get; set;}
        
        [Column("profession_id")]
        public int ProfessionId {get; set;}
        public Profession? Profession {get; set;}

        [Column("address_id")]
        public int AddressId {get; set;}
        public Address? Address {get; set;}

        public ICollection<Tithe> Tithes { get; set; } = new List<Tithe>();

    }
}
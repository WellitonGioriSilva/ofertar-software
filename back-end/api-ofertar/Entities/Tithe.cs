using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api_ofertar.Entities
{
    [Table("Tithe")]
    public class Tithe
    {

        [Column("id")]
        public int Id { get; set; }

        [Column("offeringDate")]
        public DateTime OfferingDate { get; set; }

        [Column("amount")]
        public double Amount { get; set; }

        [Column("paymentMethod")]
        public char PaymentMethod { get; set; }

        [Column("tither_id")]
        public int TitherId { get; set; }
        public Tither? Tither { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }
        public User? User { get; set; }
    }

}
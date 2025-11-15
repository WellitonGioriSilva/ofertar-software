using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api_ofertar.Enums;

namespace api_ofertar.DTOs
{
    public class TitheCreateDTO
    {
        [Required(ErrorMessage = "OfferingDate is required")]
        public DateTime OfferingDate { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "PaymentMethod is required")]
        [MaxLength(1, ErrorMessage = "PaymentMethod can't exceed 1 character")]
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;

        [Required(ErrorMessage = "TitherId is required")]
        public int TitherId { get; set; }
    }
    public class TitheUpdateDTO
    {
        public DateTime OfferingDate { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public double Amount { get; set; }

        [MaxLength(1, ErrorMessage = "PaymentMethod can't exceed 1 character")]
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;

        public int TitherId { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api_ofertar.DTOs
{
    public class AddressCreateDTO
    {
        [Required(ErrorMessage = "Street is required")]
        [MaxLength(100, ErrorMessage = "Street can't exceed 100 characters")]
        public string Street { get; set; } = String.Empty;

        [Required(ErrorMessage = "Number is required")]
        [MaxLength(10, ErrorMessage = "Number can't exceed 10 characters")]
        public string Number { get; set; } = String.Empty;

        [Required(ErrorMessage = "ZipCode is required")]
        [MaxLength(8, ErrorMessage = "ZipCode can't exceed 8 characters")]
        public string ZipCode { get; set; } = String.Empty;

        public string Complement { get; set; } = String.Empty;

        [Required(ErrorMessage = "Neighborhood is required")]
        [MaxLength(100, ErrorMessage = "Neighborhood can't exceed 100 characters")]
        public string Neighborhood { get; set; } = String.Empty;

    }
    public class AddressUpdateDTO
    {
        [MaxLength(100, ErrorMessage = "Street can't exceed 100 characters")]
        public string Street { get; set; } = String.Empty;

        [MaxLength(10, ErrorMessage = "Number can't exceed 10 characters")]
        public string Number { get; set; } = String.Empty;

        [MaxLength(8, ErrorMessage = "ZipCode can't exceed 8 characters")]
        public string ZipCode { get; set; } = String.Empty;

        public string Complement { get; set; } = String.Empty;

        [MaxLength(100, ErrorMessage = "Neighborhood can't exceed 100 characters")]
        public string Neighborhood { get; set; } = String.Empty;
    }
}
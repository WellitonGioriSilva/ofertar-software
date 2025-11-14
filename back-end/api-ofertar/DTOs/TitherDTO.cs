using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api_ofertar.Enums;

namespace api_ofertar.DTOs
{
    public class TitherCreateDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name can't exceed 100 characters")]
        public string Name { get; set; } = String.Empty;

        [MaxLength(11, ErrorMessage = "Phone can't exceed 11 characters")]
        public string Phone { get; set; } = String.Empty;

        [MaxLength(100, ErrorMessage = "Email can't exceed 100 characters")]
        public string Email { get; set; } = String.Empty;

        [Required(ErrorMessage = "BirthDate is required")]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "MaritalStatus is required")]
        public MaritalStatus MaritalStatus { get; set; } = MaritalStatus.Single;

        [MaxLength(100, ErrorMessage = "Company can't exceed 100 characters")]
        public string Company { get; set; } = String.Empty;

        // Nested spouse object to be created together with the tither
        public TitherCreateDTO? Spouse { get; set; }

        public int? ProfessionId { get; set; }

        // Nested address object to be created together with the tither
        public AddressCreateDTO? Address { get; set; }
    }
    public class TitherUpdateDTO
    {
        [MaxLength(100, ErrorMessage = "Name can't exceed 100 characters")]
        public string Name { get; set; } = String.Empty;

        [MaxLength(11, ErrorMessage = "Phone can't exceed 11 characters")]
        public string Phone { get; set; } = String.Empty;

        [MaxLength(100, ErrorMessage = "Email can't exceed 100 characters")]
        public string Email { get; set; } = String.Empty;

        public DateTime? BirthDate { get; set; }

        public MaritalStatus MaritalStatus { get; set; } = MaritalStatus.Single;

        [MaxLength(100, ErrorMessage = "Company can't exceed 100 characters")]
        public string Company { get; set; } = String.Empty;

        // Nested spouse object for updates
        public TitherUpdateDTO? Spouse { get; set; }

        public int? ProfessionId { get; set; }

        // Nested address object for updates
        public AddressUpdateDTO? Address { get; set; }
    }
}
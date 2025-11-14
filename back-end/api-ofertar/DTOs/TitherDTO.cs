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
        [MaxLength(1, ErrorMessage = "MaritalStatus can't exceed 1 character")]
        public MaritalStatus MaritalStatus { get; set; } = MaritalStatus.Single;

        [MaxLength(100, ErrorMessage = "Company can't exceed 100 characters")]
        public string Company { get; set; } = String.Empty;

        public TitherCreateDTO? SpouseId { get; set; }

        public int? ProfessionId { get; set; }

        public AddressCreateDTO? AddressId { get; set; }
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

        [MaxLength(1, ErrorMessage = "MaritalStatus can't exceed 1 character")]
        public MaritalStatus MaritalStatus { get; set; } = MaritalStatus.Single;

        [MaxLength(100, ErrorMessage = "Company can't exceed 100 characters")]
        public string Company { get; set; } = String.Empty;

        public TitherUpdateDTO? SpouseId { get; set; }

        public int? ProfessionId { get; set; }

        public AddressUpdateDTO? AddressId { get; set; }
    }
}
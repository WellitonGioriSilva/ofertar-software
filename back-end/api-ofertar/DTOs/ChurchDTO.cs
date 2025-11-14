using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api_ofertar.DTOs
{
    public class ChurchCreateDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name can't exceed 100 characters")]
        public string Name { get; set; } = String.Empty;
    }

    public class ChurchUpdateDTO
    {
        [MaxLength(100, ErrorMessage = "Name can't exceed 100 characters")]
        public string Name { get; set; } = String.Empty;
    }

    public class ChurchIsActiveDTO{
        [Required(ErrorMessage = "IsActive is required")]
        public bool isActive{get; set;}
    }
}

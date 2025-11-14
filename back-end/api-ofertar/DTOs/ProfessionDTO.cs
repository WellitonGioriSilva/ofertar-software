using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api_ofertar.DTOs
{
    public class ProfessionCreateDTO
    {
        [Required(ErrorMessage = "Code is required")]
        [MaxLength(10, ErrorMessage = "Code can't exceed 10 characters")]
        public string Code { get; set; } = String.Empty;

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name can't exceed 100 characters")]
        public string Name { get; set; } = String.Empty;
    }
    public class ProfessionUpdateDTO
    {
        [MaxLength(10, ErrorMessage = "Code can't exceed 10 characters")]
        public string Code { get; set; } = String.Empty;

        [MaxLength(100, ErrorMessage = "Name can't exceed 100 characters")]
        public string Name { get; set; } = String.Empty;
    }
}
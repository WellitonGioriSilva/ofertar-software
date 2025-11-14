using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api_ofertar.DTOs
{
    public class UserCreateDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name can't exceed 100 characters")]
        public string Name {get; set;} = String.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email {get; set;} = String.Empty;

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", 
            ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, number, and special character")]
        public string Password {get; set;} = String.Empty;

        public int? ChurchId {get; set;}

        public List<UserRoleCreateDTO>? Roles {get; set;}
    }
    public class UserUpdateDTO
    {
        [MaxLength(100, ErrorMessage = "Name can't exceed 100 characters")]
        public string Name {get; set;} = String.Empty;

        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email {get; set;} = String.Empty;

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", 
            ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, number, and special character")]
        public string Password {get; set;} = String.Empty;

        public int? ChurchId {get; set;}

        public List<UserRoleCreateDTO>? Roles {get; set;}
    }

    public class UserLoginDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email {get; set;} = String.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password {get; set;} = String.Empty;
    }

    public class UserPasswordRecoveryDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email {get; set;} = String.Empty;
    }

    public class UserPasswordResetDTO
    {
        [Required(ErrorMessage = "New password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", 
            ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, number, and special character")]
        public string NewPassword {get; set;} = String.Empty;

        [Required(ErrorMessage = "Reset token is required")]
        public string ResetToken {get; set;} = String.Empty;
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsApp.Data.Entities.DTOs.Request.Auth
{
    public class PasswordReset
    {
        [Required]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = null;
        public string Email { get; set; } = null;
        public string Token { get; set; } = null;
    }
}

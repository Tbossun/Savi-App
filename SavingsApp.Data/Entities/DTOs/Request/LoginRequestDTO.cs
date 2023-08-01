using System;
using System.Collections.Generic;
using System.Text;

namespace SavingsApp.Data.Entities.DTOs.Request
{
    public class LoginRequestDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

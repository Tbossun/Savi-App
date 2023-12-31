﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsApp.Data.Entities.DTOs.Request.Auth
{
    public class UserDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        // public string WalletId { get; set; }

    }
}

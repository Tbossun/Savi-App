﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SavingsApp.Data.Entities.Models
{
    public class IdentityType : BaseEntity
    {
        [Display(Name = "Identification Type")]
        public string Name { get; set; }

        public string IdentificationNumber { get; set; }

        public string? DocumentImageUrl { get; set; } = string.Empty;

        public ICollection<ApplicationUser> Users { get; set; }
    }
}
﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SavingsApp.Data.Entities.DTOs.Request.Kyc
{
    public class UpdateIdentityDto
    {
        [Display(Name = "Identification Type")]
        public string Name { get; set; }

        public string IdentificationNumber { get; set; }

        public IFormFile DocumentImage { get; set; }

        public string? DocumentImageUrl { get; set; } = string.Empty;
    }
}

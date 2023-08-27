using Microsoft.AspNetCore.Http;
using SavingsApp.Data.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsApp.Data.Entities.DTOs.Request
{
    public class AddPersonalSavingDto
    {
        public string UserId { get; set; }
        public string SaveName { get; set; }
        public decimal TargetAmount { get; set; }
        public DateTime TargetDate { get; set; }
        public decimal CurrentAmount { get; set; }
        public bool AutoSave { get; set; } = false;
        public decimal AutoSaveAmount { get; set; }
        public string FrequencyId { get; set; }
        public decimal MaxLimit { get; set; }
        public IFormFile? SavingsImageUrl { get; set; } = null;
        public string CategoryId { get; set; }
    }
}

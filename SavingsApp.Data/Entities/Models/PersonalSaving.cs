using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsApp.Data.Entities.Models
{
    public class PersonalSaving : BaseEntity
    {
        public string UserId { get; set; }
        public string CategoryId { get; set; }
        public string SaveName { get; set; }
        public decimal TargetAmount { get; set; }
        public DateTime TargetDate { get; set; }
        public decimal CurrentAmount { get; set; }
        public bool AutoSave { get; set; } = false;
        public decimal AutoSaveAmount { get; set; }
        public string FrequencyId { get; set; }
        public decimal MaxLimit { get; set;}
        public string SavingsImageUrl { get; set; } = string.Empty;
        public ApplicationUser User { get; set; }
        public Category category { get; set; }
        public Frequency frequency { get;}
        public ICollection<PersonalSavingsFunding> personalSavings { get; set; }
    }
}

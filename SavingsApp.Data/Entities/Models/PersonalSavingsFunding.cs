using SavingsApp.Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsApp.Data.Entities.Models
{
    public class PersonalSavingsFunding : BaseEntity
    {
        public string Id { get; set; }
        public ActionType ActionType { get; set; }
        public decimal Amount { get; set; }
        public decimal CumulativeAmount { get; set; }
        public string SavingId { get; set; }
        public PersonalSaving personalSaving { get; set; }
    }
}

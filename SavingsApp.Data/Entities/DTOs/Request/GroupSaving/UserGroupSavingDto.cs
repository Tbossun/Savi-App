using SavingsApp.Data.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsApp.Data.Entities.DTOs.Request.GroupSaving
{
    public class UserGroupSavingDto
    {
        public string Id { get; set; }
        public string SaveName { get; set; }
        public decimal ContributionAmount { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public int MemberCount { get; set; }
        public string PurposeAndGoal { get; set; }
        public string TermsAndCondition { get; set; }
        public string FrequencyId { get; set; }
        public decimal MaxLimit { get; set; }
    }
}

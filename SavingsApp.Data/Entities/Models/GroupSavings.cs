using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsApp.Data.Entities.Models
{
    public class GroupSavings  : BaseEntity
    {
        public string UserId { get; set; }
        public string SaveName { get; set; }
        public decimal ContributionAmount { get; set; }
        public DateTime ExpectedStartDate { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public decimal CurrentAmount { get; set; }
        public int MemberCount { get; set; }
        public int RunTime { get; set; }
        public int NextRunTime { get; set; }
        public string PurposeAndGoal { get; set; }
        public string TermsAndCondition { get; set; }
        public decimal AutoSaveAmount { get; set; }
        public string FrequencyId { get; set; }
        public Status GroupStatus { get; set; }
        public decimal MaxLimit { get; set; }
        public string? SavePortraitImageUrl { get; set; } = string.Empty;
        public string? SetLandScapeImageUrl { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [ForeignKey("FrequencyId")]
        public Frequency frequency { get; }
        // Define the relationship to the GroupSavingsMember
        public virtual ICollection<GroupSavingsMember> GroupSavingsMembers { get; set; }
        public ICollection<GroupSavingsFunding> groupSavingsFundings { get; set; }
    }
}

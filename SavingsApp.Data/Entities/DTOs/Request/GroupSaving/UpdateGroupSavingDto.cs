using SavingsApp.Data.Entities.Models;

namespace SavingsApp.Data.Entities.DTOs.Request.GroupSaving
{
    public class UpdateGroupSavingDto
    {
        public string GroupName { get; set; }
        public decimal ContributionAmount { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public string PurposeAndGoal { get; set; }
        public string TermsAndCondition { get; set; }
        public string FrequencyId { get; set; }
        public Status GroupStatus { get; set; }
        public decimal MaxLimit { get; set; }
    }
}
using SavingsApp.Data.Entities.Models;

namespace Savings_App.Controllers
{
    public class UpdateGroupSavingDto
    {
        public string UserId { get; set; }
        public string GroupSavingId { get; set; }
        public string GroupName { get; set; }
        public decimal ContributionAmount { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public string PurposeAndGoal { get; set; }
        public string TermsAndCondition { get; set; }
        public string FrequencyId { get; set; }
        public Status GroupStatus { get; set; }
        public decimal MaxLimit { get; set; }
        public string? SavePortraitImageUrl { get; set; } = string.Empty;
        public string? SetLandScapeImageUrl { get; set; } = string.Empty;
    }
}
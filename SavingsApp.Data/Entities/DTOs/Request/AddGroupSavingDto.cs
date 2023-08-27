using Microsoft.AspNetCore.Http;
using SavingsApp.Data.Entities.Models;

namespace SavingsApp.Data.Entities.DTOs.Request
{
    public class AddGroupSavingDto
    {
        public string UserId { get; set; }
        public string SaveName { get; set; }
        public decimal ContributionAmount { get; set; }
        public string FrequencyId { get; set; }
        public DateTime ExpectedStartDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public decimal MaxLimit { get; set; }
        public string PurposeAndGoal { get; set; }
        public int RunTime { get; set; }
        public int NextRunTime { get; set; }
        public Status GroupStatus { get; set; }
        public string TermsAndCondition { get; set; }
        public decimal AutoSaveAmount { get; set; }
        public IFormFile? SavePortraitImageUrl { get; set; } = null;
        public IFormFile? SetLandScapeImageUrl { get; set; } = null;
    }
}
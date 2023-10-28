using System.ComponentModel.DataAnnotations;

namespace SavingsApp.Data.Entities.DTOs.Request.GroupSaving
{
    public class LeaveGroupSavingDto
    {
        [Required]
        public string UserId { get; set; } // The ID of the user leaving the group saving

        [Required]
        public string GroupSavingId { get; set; }
    }
}
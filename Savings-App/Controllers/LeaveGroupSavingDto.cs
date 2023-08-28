using System.ComponentModel.DataAnnotations;

namespace Savings_App.Controllers
{
    public class LeaveGroupSavingDto
    {
        [Required]
        public string UserId { get; set; } // The ID of the user leaving the group saving

        [Required]
        public string GroupSavingId { get; set; }
    }
}
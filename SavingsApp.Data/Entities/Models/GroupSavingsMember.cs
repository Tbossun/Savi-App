using System.ComponentModel.DataAnnotations.Schema;

namespace SavingsApp.Data.Entities.Models
{
    public class GroupSavingsMember  : BaseEntity
    {
        public bool IsGroupOwner { get; set; }
        public string GroupSavingId { get; set; }
        public string UserId { get; set; }
        public int Position { get; set; }
        public DateTime? LastSavingDate { get; set; }

        [ForeignKey("GroupSavingId")]
        public virtual GroupSavings GroupSavings { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
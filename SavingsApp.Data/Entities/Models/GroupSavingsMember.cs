using System.ComponentModel.DataAnnotations.Schema;

namespace SavingsApp.Data.Entities.Models
{
    public class GroupSavingsMember  : BaseEntity
    {
        public bool IsGroupOwner { get; set; }
        public string GroupSavingId { get; set; }
        public int Position { get;}
        public DateTime? LastSavingDate { get; set; }

        [ForeignKey("GroupSavingId")]
        public virtual GroupSavings GroupSavings { get; set; }
    }
}
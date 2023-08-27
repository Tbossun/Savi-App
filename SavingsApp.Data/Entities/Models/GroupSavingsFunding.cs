using SavingsApp.Data.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SavingsApp.Data.Entities.Models
{
    public class GroupSavingsFunding : BaseEntity
    {
       // public string UserId { get; set; }
        public string GroupSavingsId { get; set; }
        public ActionType ActionType { get; set; }
        public decimal Amount { get; set; }

        /*[ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }*/

        [ForeignKey("GroupSavingId")]
        public virtual GroupSavings GroupSavings { get; set; }

    }
}
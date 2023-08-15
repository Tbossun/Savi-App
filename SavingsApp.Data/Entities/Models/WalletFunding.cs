using SavingsApp.Data.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SavingsApp.Data.Entities.Models
{
    public class WalletFunding
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }

        public string walletId { get; set; }

        public string AcctNumber { get; set; }

        public ActionType Action { get; set; }

        public string Reference { get; set; }

        public double Amount { get; set; }

        public double CumulativeAmount { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        [ForeignKey(nameof(walletId))]
        public Wallet wallet { get; set; }

    }
}

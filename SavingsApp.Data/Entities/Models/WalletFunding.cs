using SavingsApp.Data.Entities.Enums;

namespace SavingsApp.Data.Entities.Models
{
    public class WalletFunding : BaseEntity
    {

        public string walletId { get; set; }

        public ActionType Action { get; set; }

        public string Reference { get; set; }

        public double Amount { get; set; }

        public double CumulativeAmount { get; set; }

        public string Description { get; set; }

        public Wallet wallet { get; set; }

    }
}

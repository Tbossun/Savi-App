using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsApp.Data.Entities.Models
{
    public class Wallet 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }

        public string WalletId { get; set; }

        public string userId { get; set; }

        public double Balance { get; set; }
       
        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }

        [ForeignKey("userId")]
        public ApplicationUser applicationUser { get; set; }

        public ICollection<WalletFunding> WalletFundings { get; set; }

        public string SetWalletId(string phoneNumber)
        {
            if (phoneNumber.StartsWith("+234"))
            {
                phoneNumber = phoneNumber.Substring(4);
                return phoneNumber;
            }
            else if (phoneNumber.StartsWith("0"))
            {
                phoneNumber = phoneNumber.Substring(1);
                return phoneNumber;

            }

            if (phoneNumber.Length == 10 && long.TryParse(phoneNumber, out long walletId))
            {
                WalletId = walletId.ToString();
                return phoneNumber;

            }
            else
            {
                throw new Exception("Invalid Phone Number Format");
            }
        }

    }
}

using SavingsApp.Data.Entities.Enums;
using SavingsApp.Data.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsApp.Data.Repositories.IRepositories
{
    public interface IWalletFundingRepository : IRepositoryBase<WalletFunding>
    {
        void Update(WalletFunding funding);
        double GetLastCumulativeAmount(string walletId);
    }
}

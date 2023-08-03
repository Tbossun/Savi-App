using Microsoft.EntityFrameworkCore;
using SavingsApp.Data.Context;
using SavingsApp.Data.Entities.Models;
using SavingsApp.Data.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsApp.Data.Repositories
{
    public class WalletFundingRepository : RepositoryBase<WalletFunding>, IWalletFundingRepository
    {
        private SaviContext _saviDbContext;

        public WalletFundingRepository(SaviContext db) : base(db)
        {
            _saviDbContext = db;
        }

        public void Update(WalletFunding funding)
        {
            _saviDbContext.Entry(funding).State = EntityState.Modified;
            _saviDbContext.SaveChanges();
        }

        public double GetLastCumulativeAmount(string walletId)
        {
            var lastWalletFunding = _saviDbContext.WalletFundings
                .Where(wf => wf.walletId == walletId)
                .OrderByDescending(wf => wf.ModifiedAt)
                .FirstOrDefault();
            return lastWalletFunding != null ? lastWalletFunding.CumulativeAmount : 0;
        }
    }
}

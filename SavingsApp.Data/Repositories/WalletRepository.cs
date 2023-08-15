using Microsoft.EntityFrameworkCore;
using SavingsApp.Data.Context;
using SavingsApp.Data.Entities.Models;
using SavingsApp.Data.Repositories.IRepositories;

namespace SavingsApp.Data.Repositories
{
    public class WalletRepository : RepositoryBase<Wallet>, IWalletRepository
    {
        private SaviContext _saviDbContext;

        public WalletRepository(SaviContext db) : base(db)
        {
            _saviDbContext = db;
        }

        public void Update(Wallet wallet)
        {
            _saviDbContext.Entry(wallet).State = EntityState.Modified;
            _saviDbContext.SaveChangesAsync().Wait();
        }

    }
}

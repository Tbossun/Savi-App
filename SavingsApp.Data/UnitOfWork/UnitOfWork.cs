using SavingsApp.Data.Context;
using SavingsApp.Data.Repositories;
using SavingsApp.Data.Repositories.IRepositories;

namespace SavingsApp.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private SaviContext _saviDbContext;

        public IKycRepository KycRepository { get; private set; }
        public IWalletRepository WalletRepository { get; private set; }



        public UnitOfWork(SaviContext saviDbContext)
        {
            _saviDbContext = saviDbContext;
            KycRepository = new KycRepository(_saviDbContext);
            WalletRepository = new WalletRepository(_saviDbContext);

        }


        public void Save()
        {
            _saviDbContext.SaveChanges();
        }
    }
}

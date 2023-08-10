using SavingsApp.Data.Context;
using SavingsApp.Data.Entities.Models;
using SavingsApp.Data.Repositories;
using SavingsApp.Data.Repositories.IRepositories;

namespace SavingsApp.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private SaviContext _saviDbContext;

        public IKycRepository KycRepository { get; private set; }
        public IWalletRepository WalletRepository { get; private set; }
        public IWalletFundingRepository WalletFundingRepository { get; private set; }
        public IFrequencyRepository FrequencyRepository { get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }
      //  public IPersonalSavingFundingRepository PersonalSavingFundingRepository { get; private set; }
        public IPersonalSavingRepository PersonalSavingRepository { get; private set; }

        public IPersonalSavingFundingRepository PersonalSavingFundingRepository { get; private set; }

        public UnitOfWork(SaviContext saviDbContext)
        {
            _saviDbContext = saviDbContext;
            KycRepository = new KycRepository(_saviDbContext);
            WalletRepository = new WalletRepository(_saviDbContext);
            WalletFundingRepository = new WalletFundingRepository(_saviDbContext);
            FrequencyRepository = new FrequencyRepository(_saviDbContext);
            CategoryRepository = new CategoryRepository(_saviDbContext);
            PersonalSavingFundingRepository = new PersonalSavingFundingRepository(_saviDbContext);
            PersonalSavingRepository = new PersonalSavingRepository(_saviDbContext);

        }


        public void Save()
        {
            _saviDbContext.SaveChanges();
        }
    }
}

using SavingsApp.Data.Context;
using SavingsApp.Data.Repositories;
using SavingsApp.Data.Repositories.IRepositories;

namespace SavingsApp.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private SaviContext _saviDbContext;

        public IIdentityTypeRepository IdentityTypeRepository { get; private set; }

        public IOccupationRepository OccupationRepository { get; private set; }

        public IKycRepository KycRepository { get; private set; }



        public UnitOfWork(SaviContext saviDbContext)
        {
            _saviDbContext = saviDbContext;
            OccupationRepository = new OccupationRepository(_saviDbContext);
            IdentityTypeRepository = new IdentityTypeRepository(_saviDbContext);
            KycRepository = new KycRepository(_saviDbContext);
        }


        public void Save()
        {
            _saviDbContext.SaveChanges();
        }
    }
}

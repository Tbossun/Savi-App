using Microsoft.EntityFrameworkCore;
using SavingsApp.Data.Context;
using SavingsApp.Data.Entities.Models;
using SavingsApp.Data.Repositories.IRepositories;

namespace SavingsApp.Data.Repositories
{
    public class KycRepository : RepositoryBase<KYC>, IKycRepository
    {
        private SaviContext _saviDbContext;

        public KycRepository(SaviContext db) : base(db)
        {
            _saviDbContext = db;
        }

        public void Update(KYC kyc)
        {
            _saviDbContext.Entry(kyc).State = EntityState.Modified;
            _saviDbContext.SaveChanges();
        }
    }
}

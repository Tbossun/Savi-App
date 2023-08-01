using Microsoft.EntityFrameworkCore;
using SavingsApp.Data.Context;
using SavingsApp.Data.Entities.Models;
using SavingsApp.Data.Repositories.IRepositories;

namespace SavingsApp.Data.Repositories
{
    public class IdentityTypeRepository : RepositoryBase<IdentityType>, IIdentityTypeRepository
    {

        private SaviContext _saviDbContext;

        public IdentityTypeRepository(SaviContext db) : base(db)
        {
            _saviDbContext = db;
        }

        public void Update(IdentityType identityType)
        {
            _saviDbContext.Entry(identityType).State = EntityState.Modified;
            _saviDbContext.SaveChanges();
        }
    }
}

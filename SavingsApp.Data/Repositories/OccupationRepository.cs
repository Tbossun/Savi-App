using Microsoft.EntityFrameworkCore;
using SavingsApp.Data.Context;
using SavingsApp.Data.Entities.Models;
using SavingsApp.Data.Repositories.IRepositories;

namespace SavingsApp.Data.Repositories
{
    public class OccupationRepository : RepositoryBase<Occupation>, IOccupationRepository
    {
        private SaviContext _saviDbContext;

        public OccupationRepository(SaviContext db) : base(db)
        {
            _saviDbContext = db;
        }

        public void Update(Occupation occupation)
        {
            _saviDbContext.Entry(occupation).State = EntityState.Modified;
            _saviDbContext.SaveChanges();
        }
    }
}

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
    public class FrequencyRepository : RepositoryBase<Frequency>, IFrequencyRepository
    {
        private SaviContext _saviDbContext;

        public FrequencyRepository(SaviContext db) : base(db)
        {
            _saviDbContext = db;
        }

        public void Update(Frequency frequencyRepository)
        {
            _saviDbContext.Entry(frequencyRepository).State = EntityState.Modified;
            _saviDbContext.SaveChanges();
        }
    }
}

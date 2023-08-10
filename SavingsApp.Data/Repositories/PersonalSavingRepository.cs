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
    public class PersonalSavingRepository : RepositoryBase<PersonalSaving>, IPersonalSavingRepository
    {
        private SaviContext _saviDbContext;

        public PersonalSavingRepository(SaviContext db) : base(db)
        {
            _saviDbContext = db;
        }

        public void Update(PersonalSaving personalSaving)
        {
            _saviDbContext.Entry(personalSaving).State = EntityState.Modified;
            _saviDbContext.SaveChanges();
        }
    }
}

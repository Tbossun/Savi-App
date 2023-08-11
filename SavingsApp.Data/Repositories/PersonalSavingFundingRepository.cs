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
    public class PersonalSavingFundingRepository : RepositoryBase<PersonalSavingsFunding>, IPersonalSavingFundingRepository
    {
        private SaviContext _saviDbContext;

        public PersonalSavingFundingRepository(SaviContext db) : base(db)
        {
            _saviDbContext = db;
        }

        public void Update(PersonalSavingsFunding personalSavings)
        {
            _saviDbContext.Entry(personalSavings).State = EntityState.Modified;
            _saviDbContext.SaveChanges();
        }

        public decimal GetLastCumulativeAmount(string savingsId)
        {
            var lastWalletFunding = _saviDbContext.personalSavingsFundings
                .Where(wf => wf.personalSavingId == savingsId)
                .OrderByDescending(wf => wf.ModifiedAt)
                .FirstOrDefault();
            return lastWalletFunding != null ? lastWalletFunding.CumulativeAmount : 0;
        }
    }
}

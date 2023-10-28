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
    public class GroupSavingRepo : RepositoryBase<GroupSavings>, IGroupSavingRepo
    {
        private SaviContext _saviDbContext;

        public GroupSavingRepo(SaviContext db) : base(db)
        {
            _saviDbContext = db;
        }

        public async Task<IEnumerable<GroupSavings>> GetGroupSavingsByUserId(string userId)
        {
            return await _saviDbContext.GroupSavings
                .Where(gs => gs.GroupSavingsMembers.Any(member => member.UserId == userId))
                .ToListAsync();
        }

        public void Update(GroupSavings groupSavings)
        {
            _saviDbContext.Entry(groupSavings).State = EntityState.Modified;
            _saviDbContext.SaveChanges();
        }
    }
}

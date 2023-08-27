using Microsoft.EntityFrameworkCore;
using SavingsApp.Data.Context;
using SavingsApp.Data.Entities.Models;
using SavingsApp.Data.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SavingsApp.Data.Repositories
{
    public class GroupSavingMemberRepo : RepositoryBase<GroupSavingsMember>, IGroupSavingMemberRepo
    {
        private SaviContext _saviDbContext;

        public GroupSavingMemberRepo(SaviContext db) : base(db)
        {
            _saviDbContext = db;
        }

        public bool Exists(Expression<Func<GroupSavingsMember, bool>> condition)
        {
            return _saviDbContext.GroupSavingsMembers.Any(condition);
        }

        public void Update(GroupSavingsMember groupSavingsMember)
        {
            _saviDbContext.Entry(groupSavingsMember).State = EntityState.Modified;
            _saviDbContext.SaveChanges();
        }
    }
}

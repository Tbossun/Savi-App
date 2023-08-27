using SavingsApp.Data.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SavingsApp.Data.Repositories.IRepositories
{
    public interface IGroupSavingMemberRepo : IRepositoryBase<GroupSavingsMember>
    {
        void Update(GroupSavingsMember groupSavingsMember);
        bool Exists(Expression<Func<GroupSavingsMember, bool>> condition);
    }
}

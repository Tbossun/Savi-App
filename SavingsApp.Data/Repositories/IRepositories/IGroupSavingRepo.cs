using SavingsApp.Data.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsApp.Data.Repositories.IRepositories
{
    public interface IGroupSavingRepo : IRepositoryBase<GroupSavings>
    {
        void Update(GroupSavings groupSavings);
        Task<IEnumerable<GroupSavings>> GetGroupSavingsByUserId(string userId);
    }
}

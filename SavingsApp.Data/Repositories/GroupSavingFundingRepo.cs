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
    public class GroupSavingFundingRepo : RepositoryBase<GroupSavingsFunding>, IGroupSavingFundingRepo
    {
        private SaviContext _saviDbContext;

        public GroupSavingFundingRepo(SaviContext db) : base(db)
        {
            _saviDbContext = db;
        }

        public void Update(GroupSavingsFunding groupSavingsFunding)
        {
            throw new NotImplementedException();
        }
    }
}

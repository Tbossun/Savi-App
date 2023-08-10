using SavingsApp.Data.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsApp.Data.Repositories.IRepositories
{
    public interface IPersonalSavingRepository : IRepositoryBase<PersonalSaving>
    {
        void Update(PersonalSaving personalSaving);
    }
}

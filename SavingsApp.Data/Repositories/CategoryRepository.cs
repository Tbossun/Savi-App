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
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        private SaviContext _saviDbContext;

        public CategoryRepository(SaviContext db) : base(db)
        {
            _saviDbContext = db;
        }

        public void Update(Category category)
        {
            _saviDbContext.Entry(category).State = EntityState.Modified;
            _saviDbContext.SaveChanges();
        }
    }
}

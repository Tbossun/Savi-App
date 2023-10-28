using SavingsApp.Data.Entities.Models;
using System.Linq.Expressions;

namespace SavingsApp.Data.Repositories.IRepositories
{
    public interface IWalletRepository : IRepositoryBase<Wallet>
    {
        void Update(Wallet wallet);
    }
}

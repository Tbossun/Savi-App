using SavingsApp.Data.Entities.Models;

namespace SavingsApp.Data.Repositories.IRepositories
{
    public interface IWalletRepository : IRepositoryBase<Wallet>
    {
        void Update(Wallet wallet);
    }
}

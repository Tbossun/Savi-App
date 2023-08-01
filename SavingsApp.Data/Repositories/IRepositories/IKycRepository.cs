using SavingsApp.Data.Entities.Models;

namespace SavingsApp.Data.Repositories.IRepositories
{
    public interface IKycRepository : IRepositoryBase<KYC>
    {
        void Update(KYC kyc);
    }
}

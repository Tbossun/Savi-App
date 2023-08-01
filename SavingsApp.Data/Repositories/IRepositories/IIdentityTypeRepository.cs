using SavingsApp.Data.Entities.Models;

namespace SavingsApp.Data.Repositories.IRepositories
{
    public interface IIdentityTypeRepository : IRepositoryBase<IdentityType>
    {
        void Update(IdentityType identityType);
    }
}

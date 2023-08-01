using SavingsApp.Data.Entities.Enums;
using SavingsApp.Data.Entities.Models;

namespace SavingsApp.Data.Repositories.IRepositories
{
    public interface IKycRepository : IRepositoryBase<KYC>
    {
        void Update(KYC kyc);
        IEnumerable<string> GetIdentityTypes();
        IEnumerable<string> GetOccupations();
        IEnumerable<ApplicationUser> GetUsersByOccupation(Occupations occupation);
        IEnumerable<ApplicationUser> GetUsersByIdentityType(IdentificationType identityType);
    }
}

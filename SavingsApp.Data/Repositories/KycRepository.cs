using Microsoft.EntityFrameworkCore;
using SavingsApp.Data.Context;
using SavingsApp.Data.Entities.Enums;
using SavingsApp.Data.Entities.Models;
using SavingsApp.Data.Repositories.IRepositories;

namespace SavingsApp.Data.Repositories
{
    public class KycRepository : RepositoryBase<KYC>, IKycRepository
    {
        private SaviContext _saviDbContext;

        public KycRepository(SaviContext db) : base(db)
        {
            _saviDbContext = db;
        }

        public void Update(KYC kyc)
        {
            _saviDbContext.Entry(kyc).State = EntityState.Modified;
            _saviDbContext.SaveChanges();
        }

        public IEnumerable<string> GetIdentityTypes()
        {
            return Enum.GetValues(typeof(IdentificationType)).Cast<IdentificationType>()
                .Select(IdentificationType => IdentificationType.ToString()).ToList();
        }

        public IEnumerable<string> GetOccupations()
        {
            return Enum.GetValues(typeof(Occupations)).Cast<Occupations>()
                .Select(Occupations => Occupations.ToString()).ToList();
        }

        public IEnumerable<ApplicationUser> GetUsersByOccupation(Occupations occupation)
        {
            return _saviDbContext.Set<ApplicationUser>()
                .Where(u => u.Kyc.Occupation == occupation)
                .ToList();
        }

        public IEnumerable<ApplicationUser> GetUsersByIdentityType(IdentificationType identityType)
        {
            return _saviDbContext.Set<ApplicationUser>()
                .Where(u => u.Kyc.IdentityType == identityType)
                .ToList();
        }

    }
}

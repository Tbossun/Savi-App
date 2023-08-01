using SavingsApp.Data.Entities.Models;

namespace SavingsApp.Data.Repositories.IRepositories
{
    public interface IOccupationRepository : IRepositoryBase<Occupation>
    {
        void Update(Occupation occupation);
    }
}

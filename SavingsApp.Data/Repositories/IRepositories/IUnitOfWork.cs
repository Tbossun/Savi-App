using System;
using System.Collections.Generic;
using System.Text;

namespace SavingsApp.Data.Repositories.IRepositories
{
    public interface IUnitOfWork
    {
        IIdentityTypeRepository IdentityTypeRepository { get; }
        IOccupationRepository OccupationRepository { get; }
        IKycRepository KycRepository { get; }

        void Save();
    }
}

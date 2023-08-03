using System;
using System.Collections.Generic;
using System.Text;

namespace SavingsApp.Data.Repositories.IRepositories
{
    public interface IUnitOfWork
    {
        IKycRepository KycRepository { get; }
        IWalletRepository WalletRepository { get; }
        IWalletFundingRepository WalletFundingRepository { get; }

        void Save();
    }
}

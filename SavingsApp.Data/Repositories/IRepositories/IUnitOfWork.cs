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
        ICategoryRepository CategoryRepository { get; }
        IFrequencyRepository FrequencyRepository { get; }
        IPersonalSavingRepository PersonalSavingRepository { get; }
        IPersonalSavingFundingRepository PersonalSavingFundingRepository { get; }
        IGroupSavingRepo GroupSavingRepository { get; }
        IGroupSavingMemberRepo GroupSavingMemberRepo { get; }
        IGroupSavingFundingRepo GroupSavingFundingRepository { get; }

        void Save();
    }
}

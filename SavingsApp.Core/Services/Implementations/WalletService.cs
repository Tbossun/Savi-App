/*using Paystack.Net.SDK.Transactions;
using SavingsApp.Core.Services.Interfaces;
using SavingsApp.Data.Entities.Enums;
using SavingsApp.Data.Entities.Models;
using SavingsApp.Data.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsApp.Core.Services.Implementations
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WalletService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> FundWallet(string paymentReference, string WalletId)
        {
            var response = await _paystackTransaction.VerifyTransaction(paymentReference);

            // Check if the verification is successful
            if (response.status && response.data.status == "success")
            {
                var Wallet = _unitOfWork.WalletRepository.Get(W => W.WalletId == walletId);

                if (Wallet == null)
                {
                    throw new Exception("User wallet does not exist");
                }

                WalletFunding userWallet = new WalletFunding();
                userWallet.Reference = paymentReference;
                userWallet.Action = ActionType.Debit;
                userWallet.Amount = response.data.amount;
                userWallet.ModifiedAt = response.data.transaction_date;
                userWallet.CreatedAt = DateTime.UtcNow;
                userWallet.walletId = walletId;
                userWallet.CumulativeAmount += response.data.amount;
                Wallet.Balance += response.data.amount;


                _unitOfWork.WalletFundingRepository.Update(userWallet);

                _unitOfWork.Save();

                return true;
            }

            return false;
        }
    }
}
*/
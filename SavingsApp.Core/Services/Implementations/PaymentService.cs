using SavingsApp.Core.Services.Interfaces;
using SavingsApp.Data.Entities.DTOs.Response;
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
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto<bool>> FundPersonalSavings(string senderWalletId, string personalSavingsId, double amount)
        {
            var responseDto = new ResponseDto<bool>();

            var senderWallet = _unitOfWork.WalletRepository.Get(W => W.WalletId == senderWalletId);
            var receiverWallet = _unitOfWork.PersonalSavingRepository.Get(W => W.Id == personalSavingsId);

            if (senderWallet == null || receiverWallet == null)
            {
                responseDto.DisplayMessage = "Invalid sender or receiver wallet.";
                responseDto.StatusCode = 400;
                responseDto.Result = false;
                return responseDto;
            }

            // Check if the sender has sufficient balance for the transfer
            if (senderWallet.Balance < amount || senderWallet.Balance == 0)
            {
                responseDto.DisplayMessage = "Insufficient balance for the transfer.";
                responseDto.StatusCode = 400;
                responseDto.Result = false;
                return responseDto;
            }

            var senderpreviousCumulativeAmount = _unitOfWork.WalletFundingRepository.GetLastCumulativeAmount(senderWalletId);
            var SavingsCumulativeAmount = _unitOfWork.PersonalSavingFundingRepository.GetLastCumulativeAmount(personalSavingsId);

            // Debit the sender's wallet
            var senderFunding = new WalletFunding
            {
                Reference = "Deb" + GenerateUniqueReference(),
                Action = ActionType.Debit,
                Amount = amount,
                CumulativeAmount = senderpreviousCumulativeAmount - amount,
                Description = $"Local Transfer to {receiverWallet.personalSavings}",
                walletId = senderWalletId,
                ModifiedAt = DateTime.UtcNow,
            };
            _unitOfWork.WalletFundingRepository.Add(senderFunding);

            // Credit the Personal saving target wallet
            var receiverFunding = new PersonalSavingsFunding
            {
                // Reference = "Cre" + GenerateUniqueReference(),
                ActionType = ActionType.Credit,
                Amount = (decimal)amount,
                CumulativeAmount = SavingsCumulativeAmount + (decimal)amount,
                // Description = $"Transfer received from {senderWallet.WalletId}",
                personalSavingId = personalSavingsId,
                ModifiedAt = DateTime.UtcNow,
            };
            _unitOfWork.PersonalSavingFundingRepository.Add(receiverFunding);
            /// receiverWallet.Balance += amount;
            receiverWallet.ModifiedAt = DateTime.UtcNow;
            _unitOfWork.PersonalSavingRepository.Update(receiverWallet);

            _unitOfWork.Save();

            senderWallet.Balance -= amount;
            senderWallet.ModifiedAt = DateTime.UtcNow;
            _unitOfWork.WalletRepository.Update(senderWallet);



            responseDto.DisplayMessage = "Personal Saving funded successfully";
            responseDto.StatusCode = 200;
            responseDto.Result = true;

            return responseDto;
        }

        private string GenerateUniqueReference()
        {
            return "REF_" + Guid.NewGuid().ToString("N");
        }
    }
}

using SavingsApp.Core.Services.Interfaces;
using SavingsApp.Data.Entities.DTOs.Response;
using SavingsApp.Data.Entities.Enums;
using SavingsApp.Data.Entities.Models;
using SavingsApp.Data.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
            var personalSaving = _unitOfWork.PersonalSavingRepository.Get(W => W.Id == personalSavingsId);

            if (senderWallet == null || personalSaving == null)
            {
                responseDto.DisplayMessage = "Invalid sender's Wallet or personalSavings Id";
                responseDto.StatusCode = 400;
                responseDto.Result = false;
                return responseDto;
            }

            // Check if the sender has sufficient balance for the transfer
            if (senderWallet.Balance < amount || senderWallet.Balance == 0)
            {
                responseDto.DisplayMessage = "Insufficient balance!";
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
                Description = "Personal Savings funding",
                walletId = senderWallet.Id,
                AcctNumber = senderWallet.WalletId,
                ModifiedAt = DateTime.UtcNow,
            };
            _unitOfWork.WalletFundingRepository.Add(senderFunding);

            // Credit the Personal saving target wallet
            var savingsFunding = new PersonalSavingsFunding
            {
                ActionType = ActionType.Credit,
                Amount = (decimal)amount,
                CumulativeAmount = SavingsCumulativeAmount + (decimal)amount,
                Description = $"Personal savings funded from {senderWallet.WalletId}",
                personalSavingId = personalSaving.Id,
                ModifiedAt = DateTime.UtcNow,
            };
            _unitOfWork.PersonalSavingFundingRepository.Add(savingsFunding);

            personalSaving.CurrentAmount += (decimal)amount;
            personalSaving.ModifiedAt = DateTime.UtcNow;
            _unitOfWork.PersonalSavingRepository.Update(personalSaving);

            senderWallet.Balance -= amount;
            senderWallet.ModifiedAt = DateTime.UtcNow;
            _unitOfWork.WalletRepository.Update(senderWallet);

            _unitOfWork.Save();

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

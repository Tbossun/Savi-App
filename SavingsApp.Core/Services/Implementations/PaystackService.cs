using Paystack.Net.SDK.Transactions;
using SavingsApp.Core.Services.Interfaces;
using SavingsApp.Data.Entities.DTOs.Request;
using SavingsApp.Data.Entities.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paystack.Net.SDK.Models;
using Microsoft.AspNetCore.Identity;
using SavingsApp.Data.Entities.Models;
using System.Net;
using SavingsApp.Data.Entities.Enums;
using SavingsApp.Data.Repositories.IRepositories;

namespace SavingsApp.Core.Services.Implementations
{
    public class PaystackService 
    {
        private readonly string _apiKey;
        private readonly PaystackTransaction _paystackTransaction;
        private readonly IUnitOfWork _unitOfWork;
        

        public PaystackService(string apiKey, IUnitOfWork unitOfWork)
        {
            _apiKey = apiKey;
            _paystackTransaction = new PaystackTransaction(apiKey);
            _unitOfWork = unitOfWork;
            
        }

        /// <summary>
        /// Create a Paystack transaction for the given transaction request.
        /// </summary>
        /// <param name="transactionRequest">The Paystack transaction request.</param>
        /// <returns>A response containing the Paystack transaction details.</returns>
        /// <exception cref="Exception">Thrown if the Paystack API returns a null response for transaction initialization.</exception>
        public async Task<ResponseDto<PaystackTransactionResponse>> CreateTransaction(PaystackTransactionRequest transactionRequest)
        {
            var request = new TransactionInitializationRequestModel
            {
                reference = transactionRequest.Reference,
                amount = transactionRequest.Amount * 100,
                email = transactionRequest.Email,
                callbackUrl = transactionRequest.CallbackUrl
            };

            var response = await _paystackTransaction.InitializeTransaction(request);

            var responseDto = new ResponseDto<PaystackTransactionResponse>();

            if (response.data == null)
            {
                responseDto.DisplayMessage = "Paystack API returned a null response for transaction initialization.";
                responseDto.StatusCode = 400;
                responseDto.Result = null;
            }
            else
            {
                var transactionResponse = new PaystackTransactionResponse
                {
                    Status = response.status,
                    Message = response.message,
                    Data = new TransactionData
                    {
                        AuthorizationUrl = response.data.authorization_url,
                        AccessCode = response.data.access_code,
                        Reference = response.data.reference
                    }
                };

                responseDto.DisplayMessage = "Paystack transaction created successfully.";
                responseDto.StatusCode = 200;
                responseDto.Result = transactionResponse;
            }

            return responseDto;
        }



        //// <summary>
        /// The method checks if the payment reference is verified or not.
        /// </summary>
        /// <param name="paymentReference">The payment reference to verify.</param>
        /// <returns>A response containing true if the payment reference is verified, and false otherwise.</returns>
        public async Task<ResponseDto<bool>> VerifyPayment(string paymentReference)
        {
            var response = await _paystackTransaction.VerifyTransaction(paymentReference);
            var responseDto = new ResponseDto<bool>();

            if (response.status && response.data.status == "success")
            {
                responseDto.DisplayMessage = "Payment reference verified successfully.";
                responseDto.StatusCode = 200;
                responseDto.Result = true;
            }
            else
            {
                responseDto.DisplayMessage = "Transaction verification failed.";
                responseDto.StatusCode = 400;
                responseDto.Result = false;
            }

            return responseDto;
        }

        /// <summary>
        /// Fund a wallet using the provided payment reference.
        /// </summary>
        /// <param name="paymentReference">The payment reference from the Paystack transaction.</param>
        /// <param name="WalletId">The ID of the wallet to fund.</param>
        /// <param name="description">The description of the wallet funding.</param>
        /// <returns>A response indicating if the wallet was funded successfully.</returns>
        /// <exception cref="Exception">Thrown if the wallet does not exist or if the transaction verification failed.</exception>
        public async Task<ResponseDto<bool>> FundWallet(string paymentReference, string WalletId, string description)
        {
            var responseDto = new ResponseDto<bool>();

            var existingWalletFunding = _unitOfWork.WalletFundingRepository.Get(wf => wf.Reference == paymentReference);
            if (existingWalletFunding != null)
            {
                // If the payment reference already exists, return an invalid request response
                responseDto.DisplayMessage = "Invalid request. Payment reference is no longer valid";
                responseDto.StatusCode = 400;
                responseDto.Result = false;
                return responseDto;
            }
            var response = await _paystackTransaction.VerifyTransaction(paymentReference);

            // Check if the verification is successful
            if (response.status && response.data.status == "success")
            {
                var Wallet = _unitOfWork.WalletRepository.Get(W => W.WalletId == WalletId);

                if (Wallet == null)
                {
                    responseDto.DisplayMessage = "Wallet does not exist";
                    responseDto.StatusCode = 404;
                    responseDto.Result = false;
                    return responseDto;
                }
                var previousCumulativeAmount = _unitOfWork.WalletFundingRepository.GetLastCumulativeAmount(WalletId);
                var newCumulativeAmount = previousCumulativeAmount + (response.data.amount/100);
                
                WalletFunding walletFunding = new WalletFunding
                {
                    Reference = paymentReference,
                    Action = ActionType.Credit,
                    Amount = (response.data.amount/100),
                    CumulativeAmount = newCumulativeAmount,
                    Description = description,
                    walletId = Wallet.Id,
                    AcctNumber = Wallet.WalletId
                };

                // Update the wallet's balance
                Wallet.Balance += (response.data.amount / 100);
                _unitOfWork.WalletRepository.Update(Wallet);
                _unitOfWork.WalletFundingRepository.Add(walletFunding);                
                _unitOfWork.Save();

                responseDto.DisplayMessage = "Wallet Funded successfully";
                responseDto.StatusCode = 200;
                responseDto.Result = true;
            }
            else
            {
                responseDto.DisplayMessage = "Transaction verification failed";
                responseDto.StatusCode = 400;
                responseDto.Result = false;
            }
            return responseDto;
        }

        /// <summary>
        /// Transfer funds from one wallet to another.
        /// </summary>
        /// <param name="senderWalletId">The ID of the sender's wallet.</param>
        /// <param name="receiverWalletId">The ID of the receiver's wallet.</param>
        /// <param name="amount">The amount to transfer.</param>
        /// <returns>A response indicating if the transfer was successful.</returns>
        /// <exception cref="Exception">Thrown if the sender or receiver wallet is invalid, or if there is insufficient balance for the transfer.</exception>
        public async Task<ResponseDto<bool>> TransferToWallet(string senderWalletId, string receiverWalletId, double amount)
        {
            var responseDto = new ResponseDto<bool>();

            var senderWallet = _unitOfWork.WalletRepository.Get(W => W.WalletId == senderWalletId);
            var receiverWallet = _unitOfWork.WalletRepository.Get(W => W.WalletId == receiverWalletId);

            if (senderWallet == null || receiverWallet == null)
            {
                responseDto.DisplayMessage = "Invalid sender or receiver wallet.";
                responseDto.StatusCode = 400;
                responseDto.Result = false;
                return responseDto;
            }

            // Check if the sender has sufficient balance for the transfer
            if (senderWallet.Balance < amount)
            {
                responseDto.DisplayMessage = "Insufficient balance for the transfer.";
                responseDto.StatusCode = 400;
                responseDto.Result = false;
                return responseDto;
            }

            var senderpreviousCumulativeAmount = _unitOfWork.WalletFundingRepository.GetLastCumulativeAmount(senderWalletId);
            var receiverpreviousCumulativeAmount = _unitOfWork.WalletFundingRepository.GetLastCumulativeAmount(receiverWalletId);

            // Debit the sender's wallet
            var senderFunding = new WalletFunding
            {
                Reference = "Deb" + GenerateUniqueReference(),
                Action = ActionType.Debit,
                Amount = amount,
                CumulativeAmount = senderpreviousCumulativeAmount - amount,
                Description = $"Local Transfer to {receiverWallet.WalletId}",
                walletId = senderWallet.Id,
                AcctNumber = senderWallet.WalletId,
                ModifiedAt = DateTime.UtcNow,
            };
            _unitOfWork.WalletFundingRepository.Add(senderFunding);
            senderWallet.Balance -= amount;
            senderWallet.ModifiedAt = DateTime.UtcNow;
            _unitOfWork.WalletRepository.Update(senderWallet);

            // Credit the receiver's wallet
            var receiverFunding = new WalletFunding
            {
                Reference = "Cre" + GenerateUniqueReference(),
                Action = ActionType.Credit,
                Amount = amount,
                CumulativeAmount = receiverpreviousCumulativeAmount + amount,
                Description = $"Transfer received from {senderWallet.WalletId}",
                walletId = receiverWallet.Id,
                AcctNumber= receiverWallet.WalletId,
                ModifiedAt = DateTime.UtcNow,
            };
            _unitOfWork.WalletFundingRepository.Add(receiverFunding);
            receiverWallet.Balance += amount;
            receiverWallet.ModifiedAt = DateTime.UtcNow;
            _unitOfWork.WalletRepository.Update(receiverWallet);

            _unitOfWork.Save();

            responseDto.DisplayMessage = "Transfer successful";
            responseDto.StatusCode = 200;
            responseDto.Result = true;

            return responseDto;
        }


        /// <summary>
        /// Debit funds to a wallet.
        /// </summary>
        /// <param name="walletId">The ID of the wallet to credit.</param>
        /// <param name="amount">The amount to credit.</param>
        /// <param name="description">The description of the credit fund.</param>
        /// <returns>A response indicating if the credit fund was successful.</returns>
        /// <exception cref="Exception">Thrown if the wallet is invalid.</exception>
        public async Task<ResponseDto<bool>> DebitFund(string walletId, double amount, string description)
        {
            var wallet = _unitOfWork.WalletRepository.Get(W => W.WalletId == walletId);
            var responseDto = new ResponseDto<bool>();

            if (wallet == null)
            {
                responseDto.DisplayMessage = "Invalid wallet.";
                responseDto.StatusCode = 400;
                responseDto.Result = false;
                return responseDto;
            }
            var previousCumulativeAmount = _unitOfWork.WalletFundingRepository.GetLastCumulativeAmount(walletId);

            // Credit the wallet
            var walletFunding = new WalletFunding
            {
                Reference = GenerateUniqueReference(),
                Action = ActionType.Credit,
                Amount = amount,
                CumulativeAmount = previousCumulativeAmount - amount,
                Description = description,
                walletId = wallet.Id,
                AcctNumber = wallet.WalletId
            };
            _unitOfWork.WalletFundingRepository.Add(walletFunding);
            wallet.Balance -= amount;
            _unitOfWork.WalletRepository.Update(wallet);

            _unitOfWork.Save();

            responseDto.DisplayMessage = "Debit fund successful";
            responseDto.StatusCode = 200;
            responseDto.Result = true;

            return responseDto;
        }



        /// <summary>
        /// Transfer funds from one wallet to another.
        /// </summary>
        /// <param name="senderWalletId">The ID of the sender's wallet.</param>
        /// <param name="receiverWalletId">The ID of the receiver's wallet.</param>
        /// <param name="amount">The amount to transfer.</param>
        /// <returns>A response indicating if the transfer was successful.</returns>
        /// <exception cref="Exception">Thrown if the sender or receiver wallet is invalid, or if there is insufficient balance for the transfer.</exception>
       


        private string GenerateUniqueReference()
        {
            return "REF_" + Guid.NewGuid().ToString("N");
        }





    }
}

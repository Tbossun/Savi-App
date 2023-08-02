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

namespace SavingsApp.Core.Services.Implementations
{
    public class PaystackService : IPaystackService
    {
        private readonly string _apiKey;
        private readonly PaystackTransaction _paystackTransaction;

        public PaystackService(string apiKey)
        {
            _apiKey = apiKey;
            _paystackTransaction = new PaystackTransaction(apiKey);
        }

        public async Task<PaystackTransactionResponse> CreateTransaction(PaystackTransactionRequest transactionRequest)
        {
            var request = new TransactionInitializationRequestModel
            {
                reference = transactionRequest.Reference,
                amount = transactionRequest.AmountInKobo * 100,
                email = transactionRequest.Email,
                callbackUrl = transactionRequest.CallbackUrl
            };

            var response = await _paystackTransaction.InitializeTransaction(request);

            if (response.data == null)
            {
                throw new Exception("Paystack API returned a null response for transaction initialization.");
            }
            var transactionResponse = new PaystackTransactionResponse
            {
                Status = response.status,
                Message = response.message,
                Data = new TransactionData
                {
                    
                    AuthorizationUrl = response.data.authorization_url,
                    AccessCode = response.data.access_code,
                    Reference = response.data.reference,
                    
                }  
                
            };
              return transactionResponse;
        }

        public async Task<bool> VerifyPayment(string paymentReference)
        {

            var response = await _paystackTransaction.VerifyTransaction(paymentReference);

            // Check if the verification is successful
            if (response.status && response.data.status == "success")
            {
                return true;
            }

            return false;
        }
    }
}

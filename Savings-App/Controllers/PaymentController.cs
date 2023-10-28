using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SavingsApp.Core.Services.Implementations;
using SavingsApp.Data.Entities.DTOs.Request.Paystack;
using Swashbuckle.AspNetCore.Annotations;

namespace Savings_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly PaystackService _paystackService;

        public PaymentController(PaystackService paystackService)
        {
            _paystackService = paystackService;
        }

        [HttpPost("Make-Payment")]
        [SwaggerOperation(
            summary: "Create a new paystack transaction",
            description:"Create a paystack transaction"
            )]
        public async Task<IActionResult> CreateTransaction([FromBody] PaystackTransactionRequest transactionRequest)
        {
            var response = await _paystackService.CreateTransaction(transactionRequest);
            return Ok(response);
        }

        /// <summary>
        /// Verify Payment
        /// </summary>
        /// <param name="verificationRequest"></param>
        /// <returns></returns>
        [HttpPost("Verify-Payment")]
        public async Task<IActionResult> VerifyPayment([FromBody] PaystackTransactionVerificationRequest verificationRequest)
        {
            var response = await _paystackService.VerifyPayment(verificationRequest.Reference);
            return Ok(response); 
        }

        /// <summary>
        /// Fund wallet
        /// </summary>
        /// <param name="verificationRequest"></param>
        /// <param name="WalletId"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        [HttpPost("Fund-Wallet")]
        public async Task<IActionResult> FundWallet([FromBody] PaystackTransactionVerificationRequest verificationRequest, string WalletId, string description)
        {
            var response = await _paystackService.FundWallet(verificationRequest.Reference, WalletId, description);
            return Ok(response); 
        }

        /// <summary>
        /// Transfer from one wallet to another
        /// </summary>
        /// <param name="transferRequest"></param>
        /// <returns></returns>
        [HttpPost("Transfer-To-Wallet")]
        public async Task<IActionResult> TransferToWallet([FromBody] TransferToWalletRequest transferRequest)
        {
            var response = await _paystackService.TransferToWallet(transferRequest.SenderWalletId, transferRequest.ReceiverWalletId, transferRequest.Amount);
            return Ok(response); 
        }

        /// <summary>
        /// Debit a wallet
        /// </summary>
        /// <param name="walletFundingRequest"></param>
        /// <returns></returns>
        [HttpPost("Debit-Fund")]
        public async Task<IActionResult> DebitFund([FromBody] WalletFundingRequest walletFundingRequest)
        {
            var response = await _paystackService.DebitFund(walletFundingRequest.WalletId, walletFundingRequest.Amount, walletFundingRequest.Description);
            return Ok(response); 
        }

       

    }
}

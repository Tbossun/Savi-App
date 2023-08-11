using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SavingsApp.Core.Services.Implementations;
using SavingsApp.Data.Entities.DTOs.Request;

namespace Savings_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaystackService _paystackService;

        public PaymentController(PaystackService paystackService)
        {
            _paystackService = paystackService;
        }

        [HttpPost("Make-Payment")]
        public async Task<IActionResult> CreateTransaction([FromBody] PaystackTransactionRequest transactionRequest)
        {
            var response = await _paystackService.CreateTransaction(transactionRequest);
            return Ok(response);
        }

        [HttpPost("Verify-Payment")]
        public async Task<IActionResult> VerifyPayment([FromBody] PaystackTransactionVerificationRequest verificationRequest)
        {
            var response = await _paystackService.VerifyPayment(verificationRequest.Reference);
            return Ok(response); 
        }

        [HttpPost("Fund-Wallet")]
        public async Task<IActionResult> FundWallet([FromBody] PaystackTransactionVerificationRequest verificationRequest, string WalletId, string description)
        {
            var response = await _paystackService.FundWallet(verificationRequest.Reference, WalletId, description);
            return Ok(response); 
        }

        [HttpPost("Transfer-To-Wallet")]
        public async Task<IActionResult> TransferToWallet([FromBody] TransferToWalletRequest transferRequest)
        {
            var response = await _paystackService.TransferToWallet(transferRequest.SenderWalletId, transferRequest.ReceiverWalletId, transferRequest.Amount);
            return Ok(response); 
        }

        [HttpPost("Debit-Fund")]
        public async Task<IActionResult> DebitFund([FromBody] WalletFundingRequest walletFundingRequest)
        {
            var response = await _paystackService.DebitFund(walletFundingRequest.WalletId, walletFundingRequest.Amount, walletFundingRequest.Description);
            return Ok(response); 
        }

        [HttpPost("Fund-personal-Saving")]
        public async Task<IActionResult> FundPersonalSaving([FromBody] PersonalSavingsFundingRequest walletFundingRequest)
        {
            var response = await _paystackService.FundPersonalSavings(walletFundingRequest.SenderId, walletFundingRequest.SavingsId, walletFundingRequest.Amount);
            return Ok(response);
        }

    }
}

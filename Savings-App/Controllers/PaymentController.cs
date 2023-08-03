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
            var isPaymentVerified = await _paystackService.VerifyPayment(verificationRequest.Reference);

            if (isPaymentVerified.StatusCode == 200)
            {
                return Ok(new { Status = "Payment Verified", PaymentReference = verificationRequest.Reference });
            }

            return BadRequest(new { Status = "Payment Not Verified", PaymentReference = verificationRequest.Reference });
        }

        [HttpPost("Fund-Wallet")]
        public async Task<IActionResult> FundWallet([FromBody]  PaystackTransactionVerificationRequest verificationRequest, string WalletId, string description)
        {
            var fundWallet = await _paystackService.FundWallet(verificationRequest.Reference, WalletId, description);

            if (fundWallet.StatusCode == 200)
            {
                return Ok(new { Status = "Payment Verified and wallet funded", PaymentReference = verificationRequest.Reference });
            }

            return BadRequest(new { Status = "Payment Not Verified", PaymentReference = verificationRequest.Reference });
        }


        [HttpPost("Transfer-To-Wallet")]
        public async Task<IActionResult> TransferToWallet([FromBody] TransferToWalletRequest transferRequest)
        {
            var transferResult = await _paystackService.TransferToWallet(transferRequest.SenderWalletId, transferRequest.ReceiverWalletId, transferRequest.Amount);

            if (transferResult.StatusCode == 200)
            {
                return Ok(new { Status = "Transfer successful" });
            }

            return BadRequest(new { Status = "Transfer failed" });
        }

        [HttpPost("Debit-Fund")]
        public async Task<IActionResult> DebitFund([FromBody] WalletFundingRequest walletFundingRequest)
        {
            var debitResult = await _paystackService.DebitFund(walletFundingRequest.WalletId, walletFundingRequest.Amount, walletFundingRequest.Description);

            if (debitResult.StatusCode == 200)
            {
                return Ok(new { Status = "Debit fund successful" });
            }

            return BadRequest(new { Status = "Debit fund failed" });
        }

    }
}

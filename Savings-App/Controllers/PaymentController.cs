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

            if (isPaymentVerified)
            {
                return Ok(new { Status = "Payment Verified", PaymentReference = verificationRequest.Reference });
            }

            return BadRequest(new { Status = "Payment Not Verified", PaymentReference = verificationRequest.Reference });
        }
    }
}

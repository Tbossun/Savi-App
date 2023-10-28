using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SavingsApp.Data.Entities.DTOs.Request.GroupSaving;
using SavingsApp.Data.Entities.DTOs.Response;
using SavingsApp.Data.Repositories.IRepositories;

namespace Savings_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public WalletController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// Retrieves All wallets
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<APIResponse> GetAllWallets()
        {
            var wallets = _unitOfWork.WalletRepository.GetAll().ToList();
            var response = new APIResponse
            {
                StatusCode = StatusCodes.Status200OK.ToString(),
                IsSuccess = true,
                Message = "Customer's wallets retrieved successfully",
                Result = wallets
            };
            return Ok(response);
        }


        /// <summary>
        ///  Get a wallet
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">wallet retrieved successfully!</response> 
        /// <response code="404">wallet not found</response> 
        [HttpGet("wallet/{id}")]
        public ActionResult<GroupSavingsDto> GetAWallet(string id)
        {
            var wallet = _unitOfWork.WalletRepository.Get(u => u.Id == id);

            if (wallet == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new APIResponse { StatusCode = StatusCodes.Status404NotFound.ToString(), IsSuccess = false, Message = "wallet not found." });
            }

            return Ok(wallet);
        }


    }
}

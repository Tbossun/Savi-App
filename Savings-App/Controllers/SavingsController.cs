using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SavingsApp.Core.Services.Interfaces;
using SavingsApp.Data.Entities.DTOs.Response;
using SavingsApp.Data.Entities.Models;
using SavingsApp.Data.Repositories.IRepositories;
using System.Text.Json.Serialization;
using System.Text.Json;
using SavingsApp.Data.Entities.Enums;
using SavingsApp.Core.Services.Implementations;
using SavingsApp.Data.Entities.DTOs.Request.PersonalSaving;
using Microsoft.AspNetCore.Authorization;

namespace Savings_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SavingsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDocumentUploadService _uploadService;

        public SavingsController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork,
            IMapper mapper, IDocumentUploadService uploadService, IPaymentService paymentService)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _paymentService = paymentService;
        }

        /// <summary>
        /// Get all Saving Categories
        /// </summary>
        /// <returns></returns>
        [HttpGet("Saving-Categories")]
        public ActionResult<APIResponse> GetSavingCategories()
        {
            var SavingCategories = _unitOfWork.CategoryRepository.GetAll().ToList();
            var response = new APIResponse
            {
                StatusCode = StatusCodes.Status200OK.ToString(),
                IsSuccess = true,
                Message = "Saving retrieved successfully",
                Result = SavingCategories
            };
            return Ok(response);
        }


        /// <summary>
        /// Retrieve all personal saving target
        /// </summary>
        /// <returns></returns>
        [HttpGet("All-Personal-Savings")]
        public ActionResult<APIResponse> GetPersonalSavings()
        {
            var personalSavings = _unitOfWork.PersonalSavingRepository.GetAll().ToList();
            var response = new APIResponse
            {
                StatusCode = StatusCodes.Status200OK.ToString(),
                IsSuccess = true,
                Message = "Your personal saving targets retrieved successfully",
                Result = personalSavings
            };
            return Ok(response);
        }


        /// <summary>
        /// Gat a single saving target by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Personal-Saving")]
        public ActionResult<APIResponse> GetAPersonalSaving(string id)
        {
            var personalSavings = _unitOfWork.PersonalSavingRepository.Get(i =>i.Id == id);
            var response = new APIResponse
            {
                StatusCode = StatusCodes.Status200OK.ToString(),
                IsSuccess = true,
                Message = "Personal saving target retrieved successfully",
                Result = personalSavings
            };
            return Ok(response);
        }

        /// <summary>
        /// Retrieve all Saving freqencies
        /// </summary>
        /// <returns></returns>
        [HttpGet("Saving-Frequency")]
        public ActionResult<APIResponse> GetSavingFrequencies()
        {
            var Frequencies = _unitOfWork.FrequencyRepository.GetAll().ToList();
            var response = new APIResponse
            {
                StatusCode = StatusCodes.Status200OK.ToString(),
                IsSuccess = true,
                Message = "Saving frequencies retrieved successfully",
                Result = Frequencies
            };
            return Ok(response);
        }


        /// <summary>
        /// Add a new saving target
        /// </summary>
        /// <param name="saving"></param>
        /// <returns></returns>
        [HttpPost("New-Saving-Target")]
        public async Task<ActionResult<APIResponse>> AddPersonalSaving([FromForm] AddPersonalSavingDto saving)
        {
            var user = await _userManager.FindByIdAsync(saving.UserId);
            var catId =  _unitOfWork.CategoryRepository.Get(id => id.Id == saving.CategoryId);
            var frequency = _unitOfWork.FrequencyRepository.Get(id => id.Id == saving.FrequencyId);
            if (user == null || catId == null || frequency == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new APIResponse { StatusCode = StatusCodes.Status404NotFound.ToString(), IsSuccess = false, Message = "User, category or frequency not found." });
            }
            else
            {
                            
                PersonalSaving SavingDetail = _mapper.Map<PersonalSaving>(saving);
                SavingDetail.UserId = saving.UserId;
                SavingDetail.FrequencyId = saving.FrequencyId;
                SavingDetail.CategoryId = saving.CategoryId;
               
                string documentUploadUrl = string.Empty;

                if (saving.SavingsImageUrl == null)
                {
                    documentUploadUrl = string.Empty;
                    SavingDetail.SavingsImageUrl = documentUploadUrl;
                }
                else
                {
                    var documentUpload = await _uploadService.UploadImageAsync(saving.SavingsImageUrl);
                    documentUploadUrl = documentUpload.Url.ToString();
                    SavingDetail.SavingsImageUrl = documentUploadUrl;
                }

                _unitOfWork.PersonalSavingRepository.Add(SavingDetail);

                PersonalSavingsFunding savingFunds = new PersonalSavingsFunding();
                savingFunds.personalSavingId = SavingDetail.Id;
                savingFunds.ActionType = ActionType.Credit;
                savingFunds.Amount = 0;
                savingFunds.CumulativeAmount = 0;
                savingFunds.ModifiedAt = DateTime.Now;
                savingFunds.CreatedAt = DateTime.Now;
                savingFunds.Description = $"{saving.SaveName}";
                
                _unitOfWork.PersonalSavingFundingRepository.Add(savingFunds);
                _unitOfWork.Save();


                var jsonOptions = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };

                var response = new APIResponse
                {
                    StatusCode = StatusCodes.Status201Created.ToString(),
                    IsSuccess = true,
                    Message = "New Saving Target Added successfully",
                    Result = SavingDetail
                };
                return response;
            }
          // return StatusCode(StatusCodes.Status400BadRequest,
              // new APIResponse { StatusCode = StatusCodes.Status400BadRequest.ToString(), IsSuccess = false, Message = "User KYC is already completed." });
        }


        /// <summary>
        /// Fund a persomal saving target
        /// </summary>
        /// <param name="walletFundingRequest"></param>
        /// <returns></returns>
        [HttpPost("Fund-personal-Saving")]
        public async Task<IActionResult> FundPersonalSaving([FromBody] PersonalSavingsFundingRequest walletFundingRequest)
        {
            var response = await _paymentService.FundPersonalSavings(walletFundingRequest.SenderId, walletFundingRequest.SavingsId, walletFundingRequest.Amount);
            return Ok(response);
        }

    }
}

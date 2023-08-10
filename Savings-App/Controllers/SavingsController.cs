using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SavingsApp.Core.Services.Interfaces;
using SavingsApp.Data.Entities.DTOs.Request;
using SavingsApp.Data.Entities.DTOs.Response;
using SavingsApp.Data.Entities.Models;
using SavingsApp.Data.Repositories.IRepositories;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Savings_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SavingsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDocumentUploadService _uploadService;

        public SavingsController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork,
            IMapper mapper, IDocumentUploadService uploadService)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
        }


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

        [HttpPost]
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

                var documentUpload = await _uploadService.UploadImageAsync(saving.SavingsImageUrl);
                string documentUploadUrl = documentUpload.Url.ToString();

                PersonalSaving SavingDetail = _mapper.Map<PersonalSaving>(saving);
                SavingDetail.UserId = user.Id;
                SavingDetail.FrequencyId = frequency.Id;
                SavingDetail.CategoryId = catId.Id;
                SavingDetail.SavingsImageUrl = documentUploadUrl;
                SavingDetail.SavingsImageUrl = string.Empty;
                

                _unitOfWork.PersonalSavingRepository.Add(SavingDetail);
                //  user.IsKycComplete = true;
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
            //   new APIResponse { StatusCode = StatusCodes.Status400BadRequest.ToString(), IsSuccess = false, Message = "User KYC is already completed." });
        }           
      
    }
}

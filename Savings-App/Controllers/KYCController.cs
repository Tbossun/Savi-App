using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using SavingsApp.Data.Entities.Models;
using SavingsApp.Data.Repositories.IRepositories;
using SavingsApp.Core.Services.Interfaces;
using SavingsApp.Data.Entities.DTOs.Response;
using SavingsApp.Data.Entities.Enums;
using System.Net;
using SavingsApp.Data.Entities.DTOs.Request.Kyc;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;

namespace Savings_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class KYCController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDocumentUploadService _uploadService;

        public KYCController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager, IDocumentUploadService uploadService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _userManager = userManager;
        }

        
        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all Kyc Details",
            Description = "Retrieve the list of all completed KYC details"
            )]
        [Authorize]
        public ActionResult<APIResponse> GetKycDetails()
        {
            var KycDetails = _unitOfWork.KycRepository.GetAll().ToList();
            var response = new APIResponse
            {
                StatusCode = StatusCodes.Status200OK.ToString(),
                IsSuccess = true,
                Message = "Customer's details retrieved successfully",
                Result = KycDetails
            };
            return Ok(response);
        }

        /// <summary>
        ///  Get a user's KYC details
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">User Details retrieved successfully</response>
        /// <response code="404">KYC details not completed/Found</response>
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<APIResponse> GetKycDetails(string id)
        {
            var kycDetail = _unitOfWork.KycRepository.Get(u => u.UserId == id);
            if (kycDetail == null)
            {
                var notFoundResponse = new APIResponse
                {
                    StatusCode = StatusCodes.Status404NotFound.ToString(),
                    IsSuccess = false,
                    Message = "Kyc not completed"
                };
                return NotFound(notFoundResponse);
            }

            var response = new APIResponse
            {
                StatusCode = StatusCodes.Status200OK.ToString(),
                IsSuccess = true,
                Message = $"Customer with {id} details retrieved successfully",
                Result = kycDetail
            };
            return Ok(response);
        }


        /// <summary>
        /// Update User's KYC details
        /// </summary>
        /// <param name="kyc"></param>
        /// <param name="userid"></param>
        /// <response code="201">User details updated successfully</response>
        /// <response code="404">User not found</response>
        /// <response code="400">User kyc details already updated/completed</response>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<APIResponse>> UpdateKycDetails([FromForm] AddKycDto kyc, string userid)
        {
            var user = await _userManager.FindByIdAsync(userid);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new APIResponse { StatusCode = StatusCodes.Status404NotFound.ToString(), IsSuccess = false, Message = "User not found." });
            }
            else if (user.IsKycComplete is false)
            {
                if (kyc.DocumentImageUrl != null && kyc.DocumentImageUrl.Length > 0 || kyc.ProofOfAddress != null && kyc.ProofOfAddress.Length > 0)
                {
                    var documentUpload = await _uploadService.UploadImageAsync(kyc.DocumentImageUrl);
                    string documentUploadUrl = documentUpload.Url.ToString();
                    var proofOfAddress = await _uploadService.UploadImageAsync(kyc.ProofOfAddress);
                    string proofOfAddressUrl = proofOfAddress.Url.ToString();

                    KYC kycDetail = _mapper.Map<KYC>(kyc);
                    kycDetail.UserId = userid;
                    kycDetail.DocumentImageUrl = documentUploadUrl;
                    kycDetail.ProofOfAddress = proofOfAddressUrl;

                    _unitOfWork.KycRepository.Add(kycDetail);
                    user.IsKycComplete = true;
                    _unitOfWork.Save();

                    var response = new APIResponse
                    {
                        StatusCode = StatusCodes.Status201Created.ToString(),
                        IsSuccess = true,
                        Message = "kycDetail updated successfully",
                        Result = kycDetail
                    };
                   // return CreatedAtAction("GetKycDetails", new { id = user.Id }, response);
                    var options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, // Use this option to preserve references
                        WriteIndented = true // Optional: Makes the JSON output formatted for readability
                    };
                    return Content(JsonSerializer.Serialize(response, options), "application/json");
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                new APIResponse { StatusCode = StatusCodes.Status400BadRequest.ToString(), IsSuccess = false, Message = "User KYC is already completed." });
        }


        /// <summary>
        /// Retrieve all Identification type
        /// </summary>
        /// <response code="200">identification type retrieved successfully</response>
        [HttpGet("identitytypes")]
        public IActionResult GetIdentityTypes()
        {
            var identityTypes = _unitOfWork.KycRepository.GetIdentityTypes();
            return Ok(identityTypes);
        }

        /// <summary>
        /// Retrieve all Occupation type
        /// </summary>
        /// <response code="200">Occupation type retrieved successfully</response>
        [HttpGet("occupations")]
        public IActionResult GetOccupations()
        {
            var occupations = _unitOfWork.KycRepository.GetOccupations();
            return Ok(occupations);
        }

        /// <summary>
        ///  Retrieves all users by occupation type
        /// </summary>
        /// <param name="occupation"></param>
        /// <response code="200">User retrieved successfully</response>
        [HttpGet("users/{occupation}")]
        [Authorize]
        public IActionResult GetUsersByOccupation(Occupations occupation)
        {
            var users = _unitOfWork.KycRepository.GetUsersByOccupation(occupation);
            return Ok(users);
        }

        /// <summary>
        ///   Retrieves all users by Identification type
        /// </summary>
        /// <param name="identityType"></param>
        /// <response code="200">User retrieved successfully</response>
        [HttpGet("users/{identityType}")]
        [Authorize]
        public IActionResult GetUsersByIdentityType(IdentificationType identityType)
        {
            var users = _unitOfWork.KycRepository.GetUsersByIdentityType(identityType);
            return Ok(users);
        }


    }
}

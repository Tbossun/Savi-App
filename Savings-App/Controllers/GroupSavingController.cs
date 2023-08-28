using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SavingsApp.Core.Services.Interfaces;
using SavingsApp.Data.Entities.DTOs.Request;
using SavingsApp.Data.Entities.DTOs.Response;
using SavingsApp.Data.Entities.Enums;
using SavingsApp.Data.Entities.Models;
using SavingsApp.Data.Repositories.IRepositories;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Savings_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupSavingController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDocumentUploadService _uploadService;

        public GroupSavingController(IPaymentService paymentService, 
            UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork,
            IMapper mapper, IDocumentUploadService uploadService)
        {
            _paymentService = paymentService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
        }

        [HttpPost("New-Group-Saving")]
        public async Task<ActionResult<APIResponse>> AddGroupSaving([FromForm] AddGroupSavingDto saving)
        {
            var user = await _userManager.FindByIdAsync(saving.UserId);
            var frequency = _unitOfWork.FrequencyRepository.Get(id => id.Id == saving.FrequencyId);

            if (user == null || frequency == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new APIResponse { StatusCode = StatusCodes.Status404NotFound.ToString(), IsSuccess = false, Message = "User or frequency not found." });
            }
            else
            {

                GroupSavings groupSaving = _mapper.Map<GroupSavings>(saving);
                groupSaving.UserId = saving.UserId;
                groupSaving.FrequencyId = saving.FrequencyId;
                groupSaving.MemberCount = 1;

                string documentUploadUrl = string.Empty;

                if (saving.SavePortraitImageUrl == null || saving.SetLandScapeImageUrl == null)
                {
                    documentUploadUrl = string.Empty;
                    groupSaving.SavePortraitImageUrl = documentUploadUrl;
                    groupSaving.SetLandScapeImageUrl = documentUploadUrl;
                }
                else
                {
                    var documentUpload = await _uploadService.UploadImageAsync(saving.SavePortraitImageUrl);
                    documentUploadUrl = documentUpload.Url.ToString();
                    groupSaving.SavePortraitImageUrl = documentUploadUrl;
                    groupSaving.SetLandScapeImageUrl = documentUploadUrl;
                }

                _unitOfWork.GroupSavingRepository.Add(groupSaving);

                // Create GroupSavingsMember for the creator
                GroupSavingsMember creatorMember = new GroupSavingsMember
                {
                    IsGroupOwner = true,
                    GroupSavingId = groupSaving.Id,
                    Position = 1,
                    LastSavingDate = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    UserId = saving.UserId,
                    
                    
                    // Set other properties
                };
                _unitOfWork.GroupSavingMemberRepo.Add(creatorMember);

                // Create GroupSavingsFunding for the creator's funding
                GroupSavingsFunding creatorFunding = new GroupSavingsFunding
                {
                    GroupSavingsId = groupSaving.Id,
                    ActionType = ActionType.Credit,
                    Amount = 0,
                    CreatedAt = DateTime.Now,
                    ModifiedAt= DateTime.Now,
                //    CumulativeAmount = 0,
                  //  Description = $"{groupSaving.SaveName}",
                    // Set other properties
                };
                _unitOfWork.GroupSavingFundingRepository.Add(creatorFunding);

                _unitOfWork.Save();

                var response = new APIResponse
                {
                    StatusCode = StatusCodes.Status201Created.ToString(),
                    IsSuccess = true,
                    Message = "New Group Saving Added successfully",
                    Result = groupSaving
                };
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve, // Use this option to preserve references
                    WriteIndented = true // Optional: Makes the JSON output formatted for readability
                };
                return Content(JsonSerializer.Serialize(response, options), "application/json");
            }
        }



        [HttpPost("Join-Group-Saving")]
        public async Task<ActionResult<APIResponse>> JoinGroupSaving([FromForm] JoinGroupSavingDto joinRequest)
        {
            var user = await _userManager.FindByIdAsync(joinRequest.UserId);
            var groupSaving = _unitOfWork.GroupSavingRepository.Get(id => id.Id == joinRequest.GroupSavingId);

            if (user == null || groupSaving == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new APIResponse { StatusCode = StatusCodes.Status404NotFound.ToString(), IsSuccess = false, Message = "User or group saving not found." });
            }

            // Check if the user is already a member of the group
            bool isAlreadyMember = _unitOfWork.GroupSavingMemberRepo.Exists(member => member.UserId == joinRequest.UserId && member.GroupSavingId == joinRequest.GroupSavingId);
            if (isAlreadyMember)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new APIResponse { StatusCode = StatusCodes.Status400BadRequest.ToString(), IsSuccess = false, Message = "User is already a member of the group." });
            }

            // If the user is not a member, add them as a member
            GroupSavingsMember newMember = new GroupSavingsMember
            {
                IsGroupOwner = false,
                GroupSavingId = joinRequest.GroupSavingId,
                Position = groupSaving.MemberCount + 1,
                LastSavingDate = DateTime.Now,
                CreatedAt = DateTime.Now,                                                                                                                 
                ModifiedAt = DateTime.Now,
                UserId = joinRequest.UserId,
                // Set other properties
            };
            _unitOfWork.GroupSavingMemberRepo.Add(newMember);

            // Update the MemberCount of the group
            groupSaving.MemberCount += 1; // Increment the member count
            _unitOfWork.GroupSavingRepository.Update(groupSaving); // Update the entity
            _unitOfWork.Save();

            var response = new APIResponse
            {
                StatusCode = StatusCodes.Status201Created.ToString(),
                IsSuccess = true,
                Message = "User successfully joined the group saving.",
                Result = newMember
            };
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve, // Use this option to preserve references
                WriteIndented = true // Optional: Makes the JSON output formatted for readability
            };
            return Content(JsonSerializer.Serialize(response, options), "application/json");
        }


    }
}

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SavingsApp.Core.Services.Interfaces;
using SavingsApp.Data.Entities.DTOs.Response;
using SavingsApp.Data.Entities.Enums;
using SavingsApp.Data.Entities.Models;
using SavingsApp.Data.Repositories.IRepositories;
using System.Text.Json.Serialization;
using System.Text.Json;
using SavingsApp.Data.Entities.DTOs.Request.GroupSaving;
using Microsoft.AspNetCore.Authorization;

namespace Savings_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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


        /// <summary>
        /// Create new Saving Group
        /// </summary>
        /// <param name="saving"></param>
        /// <response code="201">Saving group Created successfully</response> 
        /// <response code="404">User or Frequency not found</response> 
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


        /// <summary>
        /// Join a Saving Group
        /// </summary>
        /// <param name="joinRequest"></param>
        /// <response code="202">User hoin successfully</response> 
        /// <response code="404">User already the in group</response> 
        /// <response code="404">Saving group or user not found</response> 
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
                StatusCode = StatusCodes.Status202Accepted.ToString(),
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

        /// <summary>
        /// Leave a Savings Group
        /// </summary>
        /// <param name="leaveRequest"></param>
        /// <response code="200">User left group successfully!</response> 
        /// <response code="404">Saving group not found</response> 
        /// <response code="403">User cannot leave group</response> 
        /// <response code="400">User is not a member of the group.!</response> 
        [HttpPost("Leave-Group-Saving")]
        public async Task<ActionResult<APIResponse>> LeaveGroupSaving([FromForm] LeaveGroupSavingDto leaveRequest)
        {
            var user = await _userManager.FindByIdAsync(leaveRequest.UserId);
            var groupSaving = _unitOfWork.GroupSavingRepository.Get(id => id.Id == leaveRequest.GroupSavingId);

            if (user == null || groupSaving == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new APIResponse { StatusCode = StatusCodes.Status404NotFound.ToString(), IsSuccess = false, Message = "User or group saving not found." });
            }

            // Check if the user is the group owner
            if (groupSaving.UserId == leaveRequest.UserId)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new APIResponse { StatusCode = StatusCodes.Status403Forbidden.ToString(), IsSuccess = false, Message = "Group owner cannot leave the group." });
            }

            // Check if the user is a member of the group
            var groupMember = _unitOfWork.GroupSavingMemberRepo.Get(member => member.UserId == leaveRequest.UserId && member.GroupSavingId == leaveRequest.GroupSavingId);
            if (groupMember == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new APIResponse { StatusCode = StatusCodes.Status400BadRequest.ToString(), IsSuccess = false, Message = "User is not a member of the group." });
            }

            // Remove the user as a member
            _unitOfWork.GroupSavingMemberRepo.Remove(groupMember);

            // Decrement the MemberCount of the group
            groupSaving.MemberCount -= 1;
            _unitOfWork.GroupSavingRepository.Update(groupSaving);
            _unitOfWork.Save();

            var response = new APIResponse
            {
                StatusCode = StatusCodes.Status200OK.ToString(),
                IsSuccess = true,
                Message = "User successfully left the group saving.",
                Result = null // No need to include result in this case
            };
            return response;
        }


       
        /// <summary>
        /// Get all Saving Groups
        /// </summary>
        /// <response code="200">Saving Groups retrieved successfully!</response> 
        [HttpGet("All-Group-Savings")]
        public ActionResult<IEnumerable<GroupSavingsDto>> GetAllGroupSavings()
        {
            var groupSavingsList = _unitOfWork.GroupSavingRepository.GetAll();
            var groupSavingsDtoList = _mapper.Map<IEnumerable<GroupSavingsDto>>(groupSavingsList);

            return Ok(groupSavingsDtoList);
        }

        /// <summary>
        ///  Get a Saving Group
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Saving Group retrieved successfully!</response> 
        /// <response code="404">Saving group not found</response> 
        [HttpGet("Group-Saving/{id}")]
        public ActionResult<GroupSavingsDto> GetGroupSaving(string id)
        {
            var groupSaving = _unitOfWork.GroupSavingRepository.Get(u => u.Id == id);

            if (groupSaving == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new APIResponse { StatusCode = StatusCodes.Status404NotFound.ToString(), IsSuccess = false, Message = "Group saving not found." });
            }

            var groupSavingDto = _mapper.Map<GroupSavingsDto>(groupSaving);

            return Ok(groupSavingDto);
        }

        /// <summary>
        ///  Get user's Saving Group
        /// </summary>
        /// <param name="userId"></param>
        /// <response code="200">Saving Groups retrieved successfully!</response> 
        [HttpGet("User-Group-Savings/{userId}")]
        public ActionResult<IEnumerable<UserGroupSavingDto>> GetUserGroupSavings(string userId)
        {
            var userGroupSavings = _unitOfWork.GroupSavingRepository.GetGroupSavingsByUserId(userId);
            var userGroupSavingsDtoList = _mapper.Map<IEnumerable<UserGroupSavingDto>>(userGroupSavings);

            return Ok(userGroupSavingsDtoList);
        }


        /// <summary>
        /// Delete  a Saving Group
        /// </summary>
        /// <param name="deleteRequest"></param>
        /// <response code="200">Group Deleted successfully!</response> 
        /// <response code="404">Group not found</response> 
        /// <response code="403">User can't delete group as user is not the group owner</response> 
        [HttpDelete("Delete-Group-Saving")]
        public async Task<ActionResult<APIResponse>> DeleteGroupSaving([FromBody] DeleteGroupSavingDto deleteRequest)
        {
            var user = await _userManager.FindByIdAsync(deleteRequest.UserId);
            var groupSaving = _unitOfWork.GroupSavingRepository.Get(id => id.Id == deleteRequest.GroupSavingId);

            if (user == null || groupSaving == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new APIResponse { StatusCode = StatusCodes.Status404NotFound.ToString(), IsSuccess = false, Message = "User or group saving not found." });
            }

            // Check if the user is the owner of the group
            bool isGroupOwner = _unitOfWork.GroupSavingMemberRepo.Exists(member => member.UserId == user.Id && member.GroupSavingId == groupSaving.Id && member.IsGroupOwner);
            if (!isGroupOwner)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new APIResponse { StatusCode = StatusCodes.Status403Forbidden.ToString(), IsSuccess = false, Message = "User is not the owner of the group." });
            }

            // Delete the group saving and related entities
            _unitOfWork.GroupSavingMemberRepo.RemoveRange(groupSaving.GroupSavingsMembers);
            _unitOfWork.GroupSavingFundingRepository.RemoveRange(groupSaving.groupSavingsFundings);
            _unitOfWork.GroupSavingRepository.Remove(groupSaving);
            _unitOfWork.Save();

            var response = new APIResponse
            {
                StatusCode = StatusCodes.Status200OK.ToString(),
                IsSuccess = true,
                Message = "Group Saving successfully deleted.",
                Result = null // You can provide additional data here if needed
            };

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            return Content(JsonSerializer.Serialize(response, options), "application/json");
        }

        /// <summary>
        /// Update a Saving Group details
        /// </summary>
        /// <param name="updateRequest"></param>
        /// <param name="UserId"></param>
        /// <param name="GroupSavingId"></param>
        /// <response code="200">Saving Groups updated successfully!</response> 
        /// <response code="403">User can't update group details</response> 
        /// <response code="404">Group Not found</response> 
        [HttpPut("Update-Group-Saving")]
        public async Task<ActionResult<APIResponse>> UpdateGroupSaving([FromBody] UpdateGroupSavingDto updateRequest, string UserId, string GroupSavingId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            var groupSaving = _unitOfWork.GroupSavingRepository.Get(id => id.Id == GroupSavingId);

            if (user == null || groupSaving == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new APIResponse { StatusCode = StatusCodes.Status404NotFound.ToString(), IsSuccess = false, Message = "User or group saving not found." });
            }

            // Check if the user is the owner of the group
            bool isGroupOwner = _unitOfWork.GroupSavingMemberRepo.Exists(member => member.UserId == user.Id && member.GroupSavingId == groupSaving.Id && member.IsGroupOwner);
            if (!isGroupOwner)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new APIResponse { StatusCode = StatusCodes.Status403Forbidden.ToString(), IsSuccess = false, Message = "User is not the owner of the group." });
            }

            // Update the group saving properties
            groupSaving.SaveName = updateRequest.GroupName;
            groupSaving.ContributionAmount = updateRequest.ContributionAmount;
            groupSaving.FrequencyId = updateRequest.FrequencyId;


            _unitOfWork.GroupSavingRepository.Update(groupSaving);
            _unitOfWork.Save();

            var response = new APIResponse
            {
                StatusCode = StatusCodes.Status200OK.ToString(),
                IsSuccess = true,
                Message = "Group Saving successfully updated.",
                Result = groupSaving // Return the updated group saving
            };

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            return Content(JsonSerializer.Serialize(response, options), "application/json");
        }



    }
}

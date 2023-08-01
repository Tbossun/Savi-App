using SavingsApp.Data.Entities.Enums;
using SavingsApp.Data.Entities.Models;

namespace SavingsApp.Data.Entities.DTOs.Response
{
    public class KycResponseDto
    {
        public string UserId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Occupations Occupation { get; set; }
        public string Address { get; set; }
        public string BVN { get; set; }
        public IdentificationType IdentityType { get; set; }
        public ApplicationUser User { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using SavingsApp.Data.Entities.Enums;

namespace SavingsApp.Data.Entities.DTOs.Request.Kyc
{
    public class AddKycDto
    {
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Occupations Occupation { get; set; }
        public string Address { get; set; }
        public string BVN { get; set; }
        public IdentificationType IdentityType { get; set; }
        public IFormFile DocumentImageUrl { get; set; } = null;
        public IFormFile ProofOfAddress { get; set; } = null;
    }
}

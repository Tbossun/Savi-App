using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsApp.Data.Entities.DTOs.Request.Auth
{
    public class RegRequestDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public string OccupationId { get; set; }
        public string Address { get; set; }
        public string BVN { get; set; }
        public string IdentityTypeId { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;
        public string ProofOfAddressUrl { get; set; }
    }
}

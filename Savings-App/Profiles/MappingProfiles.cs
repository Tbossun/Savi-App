using AutoMapper;
using SavingsApp.Data.Entities.DTOs.Request;
using SavingsApp.Data.Entities.Models;
using SavingsApp.Data.Migrations;

namespace Savings_App.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ApplicationUser, SignUpDto>().ReverseMap();
            CreateMap<KYC, AddKycDto>().ReverseMap();
            CreateMap<PersonalSaving, AddPersonalSavingDto>().ReverseMap();
        }
    }
}

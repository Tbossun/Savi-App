using AutoMapper;
using SavingsApp.Data.Entities.DTOs.Request;
using SavingsApp.Data.Entities.Models;

namespace Savings_App.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ApplicationUser, SignUpDto>().ReverseMap();
            CreateMap<KYC, AddKycDto>().ReverseMap();
            CreateMap<PersonalSaving, AddPersonalSavingDto>().ReverseMap();
            CreateMap<Wallet, WalletUpdate>().ReverseMap();
            CreateMap<WalletUpdate, Wallet>();
        }
    }
}

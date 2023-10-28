using AutoMapper;
using SavingsApp.Data.Entities.DTOs.Request.Auth;
using SavingsApp.Data.Entities.DTOs.Request.GroupSaving;
using SavingsApp.Data.Entities.DTOs.Request.Kyc;
using SavingsApp.Data.Entities.DTOs.Request.Paystack;
using SavingsApp.Data.Entities.DTOs.Request.PersonalSaving;
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
            CreateMap<GroupSavings, AddGroupSavingDto>().ReverseMap();
            CreateMap<GroupSavings, GroupSavingsDto>().ReverseMap();
            CreateMap<UserGroupSavingDto, GroupSavings>().ReverseMap();
        }
    }
}

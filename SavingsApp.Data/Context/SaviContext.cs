using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SavingsApp.Data.Entities.Enums;
using SavingsApp.Data.Entities.Models;

namespace SavingsApp.Data.Context
{
    public class SaviContext : IdentityDbContext<ApplicationUser>
    {
        public SaviContext(DbContextOptions<SaviContext> options) : base(options)
        {
        }

        public DbSet<KYC> kYCs { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletFunding> WalletFundings { get; set; }
        public DbSet<PersonalSaving> personalSavings { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Frequency> frequencies { get; set; } 
        public DbSet<PersonalSavingsFunding> personalSavingsFundings { get; set; }
         


    }
}

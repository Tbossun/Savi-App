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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure ApplicationUser and KYC relationship
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(user => user.Kyc)
                .WithOne(kyc => kyc.User)
                .HasForeignKey<KYC>(kyc => kyc.UserId);

            // Configure Category and PersonalSaving relationship
            modelBuilder.Entity<Category>()
                .HasMany(category => category.personalSavings)
                .WithOne(personalSaving => personalSaving.category);

            // Configure PersonalSaving and Frequency relationship
            modelBuilder.Entity<PersonalSaving>()
                .HasOne(personalSaving => personalSaving.frequency)
                .WithMany()
                .HasForeignKey(personalSaving => personalSaving.FrequencyId);

            // Configure PersonalSaving and ApplicationUser relationship
            modelBuilder.Entity<PersonalSaving>()
                .HasOne(personalSaving => personalSaving.User)
                .WithMany(user => user.PersonalSavings)
                .HasForeignKey(personalSaving => personalSaving.UserId);

            // Configure PersonalSaving and PersonalSavingsFunding relationship
            modelBuilder.Entity<PersonalSaving>()
                .HasMany(personalSaving => personalSaving.personalSavings)
                .WithOne(personalSavingsFunding => personalSavingsFunding.personalSaving);

            // Configure PersonalSavingsFunding and PersonalSaving relationship
            modelBuilder.Entity<PersonalSavingsFunding>()
                .HasOne(personalSavingsFunding => personalSavingsFunding.personalSaving)
                .WithMany(personalSaving => personalSaving.personalSavings)
                .HasForeignKey(personalSavingsFunding => personalSavingsFunding.personalSavingId);


            // Configure Wallet and WalletFunding relationship
            modelBuilder.Entity<Wallet>()
                .HasMany(wallet => wallet.WalletFundings)
                .WithOne(walletFunding => walletFunding.wallet);

            // Configure WalletFunding and Wallet relationship
            modelBuilder.Entity<WalletFunding>()
                .HasOne(walletFunding => walletFunding.wallet)
                .WithMany(wallet => wallet.WalletFundings)
                .HasForeignKey(walletFunding => walletFunding.walletId);

            // ... (Configure other relationships)

            // Specify unique constraints and indexes as needed

        }


    }
}

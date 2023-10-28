using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using SavingsApp.Data.Context;
using SavingsApp.Data.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SavingsApp.Data.Seeding
{
    public static class Seeder
    {
        public static async void SeedDataBase(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, SaviContext context)
        {
            try
            {
                await context.Database.EnsureCreatedAsync();
                if (!context.frequencies.Any())
                {
                    string roles = File.ReadAllText(@"JsonFiles/Roles.json");
                    List<IdentityRole> listOfRoles = JsonConvert.DeserializeObject<List<IdentityRole>>(roles);

                    foreach (var role in listOfRoles)
                    {
                        await roleManager.CreateAsync(role);
                    }

                    string categories = File.ReadAllText(@"JsonFiles/Categories.json");
                    List<Category> listOfCategories = JsonConvert.DeserializeObject<List<Category>>(categories);


                    foreach (var cat in listOfCategories)
                    {
                        await context.categories.AddAsync(cat);
                    }

                    string frequencies = File.ReadAllText(@"JsonFiles/Frequencies.json");
                    List<Frequency> listOfFrequencies = JsonConvert.DeserializeObject<List<Frequency>>(frequencies);

                    foreach (var frequency in listOfFrequencies)
                    {
                        await context.frequencies.AddAsync(frequency);
                    }
                }
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

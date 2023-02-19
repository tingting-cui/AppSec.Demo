using MoveShareApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace MoveShareApplication.Data
{
    public static class InitializeDB
    {
        /// <summary>
        /// Created Follwoing 3 users 
        /// [Role - Login email - Login Credential]
        ///     Administrator - Alice@moveshare.com - A$$leT0ee
        ///     User - Bob@moveshare.com - A$$leT0ee
        ///     User - Tingting@moveshare.com - A$$leT0ee
        /// <returns></returns>

        public static async Task InitializeRole(IServiceProvider serviceProvider)
        {
            var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            var role1 = new IdentityRole
            {
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            };
            var role2 = new IdentityRole
            {
                Name = "User",
                NormalizedName = "USER"
            };


            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (!context.Roles.Any(r => r.Name == role1.Name))
            {
                await roleManager.CreateAsync(role1);
            }
            if (!context.Roles.Any(r => r.Name == role2.Name))
            {
                await roleManager.CreateAsync(role2);
            }

            context.SaveChangesAsync();

        }

        public static async Task InitializeUser(IServiceProvider serviceProvider)
        {
            var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            //a hasher to hash the password before seeding the user to the db
            var hasher = new PasswordHasher<IdentityUser>();

            //Seeding the Users to AspNetUsers table

            var userAdmin = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(),// primary key
                UserName = "Alice@moveshare.com",
                NormalizedUserName = "ALICE@MOVESHARE.COM",
                Email = "Alice@moveshare.com",
                NormalizedEmail = "ALICE@MOVESHARE.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };


            var user1 = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(),// primary key
                UserName = "Bob@moveshare.com",
                NormalizedUserName = "BOB@MOVESHARE.COM",
                Email = "Bob@moveshare.com",
                NormalizedEmail = "BOB@MOVESHARE.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var user2 = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(),// primary key
                UserName = "Tingting@moveshare.com",
                NormalizedUserName = "TINGTING@MOVESHARE.COM",
                Email = "Tingting@moveshare.com",
                NormalizedEmail = "TINGTING@MOVESHARE.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            //Add User & assign Role management
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
            var passwordhasher = new PasswordHasher<IdentityUser>();

            if (!context.Users.Any(u => u.UserName == userAdmin.UserName))
            {
                
                userAdmin.PasswordHash = passwordhasher.HashPassword(userAdmin, "A$$leT0ee");

                await userManager.CreateAsync(userAdmin);
                await userManager.AddToRoleAsync(userAdmin, "Administrator");
            }
            if (!context.Users.Any(u => u.UserName == user1.UserName))
            {
                user1.PasswordHash = passwordhasher.HashPassword(user1, "A$$leT0ee");

                await userManager.CreateAsync(user1);
                await userManager.AddToRoleAsync(user1, "User");
            }
            if (!context.Users.Any(u => u.UserName == user2.UserName))
            {
                user2.PasswordHash = passwordhasher.HashPassword(user2, "A$$leT0ee");

                await userManager.CreateAsync(user2);
                await userManager.AddToRoleAsync(user2, "User");
            }
           
            context.SaveChangesAsync();
        }


        public static void InitializeItem(IServiceProvider serviceProvider)
        {
            var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            if (context.Item.Any()) {
                return;   // DB has been seeded
            }

            string admin_Alice_id= context.Users.FirstOrDefault(u => u.UserName == "Alice@moveshare.com").Id;
            string Bob_id = context.Users.FirstOrDefault(u => u.UserName == "Bob@moveshare.com").Id;
            string Tingting_id = context.Users.FirstOrDefault(u => u.UserName == "Tingting@moveshare.com").Id;

            var items = new Item[]
            {
                new Item() { Item_id=Guid.NewGuid().ToString(),Owner_id=admin_Alice_id, Name="Dish",Description="With colorful Fish print", Created_at=DateTime.Now,LastUpdate_at=DateTime.Now,Available=true,Quantity=10,Location="Luxembourg",PickUpNote="Pls come on Sunday afternoon at adress @"},
                new Item() { Item_id=Guid.NewGuid().ToString(),Owner_id=Bob_id, Name="Apple",Description="fresh", Created_at=DateTime.Now,LastUpdate_at=DateTime.Now,Available=true,Quantity=10,Location="Luxembourg farm @Bettembourg"},
                new Item() { Item_id=Guid.NewGuid().ToString(),Owner_id=Bob_id, Name="Sofa",Description="4 seats, leather", Created_at=DateTime.Now,LastUpdate_at=DateTime.Now,Available=true,Quantity=1,Location="Luxembourg",PickUpNote="Pls come on Sunday afternoon at adress @"},
                new Item() { Item_id=Guid.NewGuid().ToString(),Owner_id=Tingting_id, Name="Wine glasses", Created_at=DateTime.Now,LastUpdate_at=DateTime.Now,Available=true,Quantity=7,Location="Luxembourg",PickUpNote="Pls contact me in advance. number: 1234567"},
                
            };

            foreach (Item i in items)
            {
                context.Item.Add(i);
            }
            context.SaveChanges();

        }

    }
}
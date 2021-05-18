using MeowStory.Domain.Entities;
using MeowStory.Infrastructure.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MeowStory.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedUsersAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            #region Seed Admins
            var administratorRole = new IdentityRole("Administrator");

            if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
            {
                await roleManager.CreateAsync(administratorRole);
            }

            var administrator = new ApplicationUser { UserName = "admin@meowstory.com", Email = "admin@meowstory.com" };

            if (userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                await userManager.CreateAsync(administrator, "Pass@123X");
                await userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }

            #endregion

            #region Seed Authors

            var autherRole = new IdentityRole("Author");

            if (roleManager.Roles.All(r => r.Name != autherRole.Name))
            {
                await roleManager.CreateAsync(autherRole);
            }

            for (int i = 0; i < 3; i++)
            {
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = $"user{i}",
                    Email = $"user{i}@meowstory.com"
                };
                if (!userManager.Users.Any(u => u.UserName == user.UserName))
                {
                    await userManager.CreateAsync(user, "Pass@123X");

                    await userManager.AddToRolesAsync(user, new[] { autherRole.Name });

                    context.Authors.Add(new Author(user.Id, $"A <firstname> {i}", $"A <lastname> {i}"));
                }
            }
            #endregion

            #region Seed Reviewers

            var reviewerRole = new IdentityRole("Reviewer");

            if (roleManager.Roles.All(r => r.Name != reviewerRole.Name))
            {
                await roleManager.CreateAsync(reviewerRole);
            }

            for (int i = 0; i < 3; i++)
            {
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = $"user{i}",
                    Email = $"reviewer{i}@meowstory.com"
                };

                if (!userManager.Users.Any(u => u.UserName == user.UserName))
                {
                    await userManager.CreateAsync(user, "Pass@123X");

                    await userManager.AddToRolesAsync(user, new[] { reviewerRole.Name });

                    context.Reviewers.Add(new Reviewer(user.Id, $"R <firstname> {i}", $"R <lastname> {i}"));
                }

            }
            await context.SaveChangesAsync();
            #endregion
        }

        public static async Task SeedStoriesAsync(ApplicationDbContext context)
        {

        }
    }
}

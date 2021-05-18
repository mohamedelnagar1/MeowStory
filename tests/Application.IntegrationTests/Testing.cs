using MeowStory.Application.Common.Interfaces;
using MeowStory.Infrastructure.Authentication;
using MeowStory.Infrastructure.Persistence;
using MeowStory.WebApi;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Respawn;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MeowStory.Domain.Entities;

[SetUpFixture]
public class Testing
{
    private static IConfigurationRoot _configuration;
    private static IServiceScopeFactory _scopeFactory;
    private static Checkpoint _checkpoint;
    private static string _currentUserId;

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .AddEnvironmentVariables();

        _configuration = builder.Build();

        var startup = new Startup(_configuration);

        var services = new ServiceCollection();

        services.AddSingleton(Mock.Of<IWebHostEnvironment>(w => w.EnvironmentName == "Development" && w.ApplicationName == "MeowStory.WebApi"));

        services.AddLogging();

        startup.ConfigureServices(services);

        // Replace service registration for ICurrentUserService
        // Remove existing registration
        var currentUserServiceDescriptor = services.FirstOrDefault(d =>
            d.ServiceType == typeof(ICurrentUserService));

        services.Remove(currentUserServiceDescriptor);

        // Register testing version
        services.AddTransient(provider =>
            Mock.Of<ICurrentUserService>(s => s.UserId == _currentUserId));

        _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();

        _checkpoint = new Checkpoint
        {
            TablesToIgnore = new[] { "__EFMigrationsHistory" }
        };

        EnsureDatabase();
    }

    private static void EnsureDatabase()
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
        context.Database.EnsureDeleted();
        context.Database.Migrate();
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetService<ISender>();

        return await mediator.Send(request);
    }

    public static async Task<string> RunAsDefaultUserAsync()
    {
        return await RunAsUserAsync("user@meowstory.com", "Pass@123X", new string[] { });
    }

    public static async Task<string> RunAsAdministratorAsync()
    {
        return await RunAsUserAsync("admin@meowstory.com", "Pass@123X", new[] { "Administrator" });
    }

    public static async Task<string> RunAsAuthorAsync()
    {
        var userId = await RunAsUserAsync("author@meowstory.com", "Pass@123X", new[] { "Author" });

        var author = await FindAsync<Author>(userId);
        if (author == null)
            await AddAsync(new Author(userId, "test", "author"));
        return userId;
    }

    public static async Task<string> RunAsReviewerAsync()
    {
        var userId = await RunAsUserAsync("reviewer@meowstory.com", "Pass@123X", new[] { "Reviewer" });
        var reviewer = await FindAsync<Reviewer>(userId);
        if (reviewer == null)
            await AddAsync(new Reviewer(userId, "test", "reviewer"));
        return userId;
    }

    public static async Task<string> RunAsUserAsync(string userName, string password, string[] roles)
    {
        using var scope = _scopeFactory.CreateScope();

        var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

        ApplicationUser user = await userManager.FindByNameAsync(userName);

        if (user != null)
        {
            _currentUserId = user.Id;
            return _currentUserId;
        }

        user = new ApplicationUser { UserName = userName, Email = userName };

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join("|", result.Errors));
        }

        if (roles.Any())
        {
            var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            await userManager.AddToRolesAsync(user, roles);
        }

        if (result.Succeeded)
        {
            _currentUserId = user.Id;
            return _currentUserId;
        }

        var errors = string.Join(Environment.NewLine, result.ToApplicationResult().Errors);

        throw new Exception($"Unable to create {userName}.{Environment.NewLine}{errors}");
    }

    public static async Task ResetState()
    {
        await _checkpoint.Reset(_configuration.GetConnectionString("DefaultConnection"));
        _currentUserId = null;
    }

    public static async Task<TEntity> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public static async Task<int> CountAsync<TEntity>() where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        return await context.Set<TEntity>().CountAsync();
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
        context.Database.EnsureDeleted();
    }
}

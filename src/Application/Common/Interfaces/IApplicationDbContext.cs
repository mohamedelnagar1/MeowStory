using MeowStory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace MeowStory.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Author> Authors { get; set; }
        DbSet<Reviewer> Reviewers { get; set; }
        DbSet<Genre> Genres { get; set; }
        DbSet<Story> Stories { get; set; }
        DbSet<Comment> Comments { get; set; }
        DbSet<UserProfile> UserProfiles { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
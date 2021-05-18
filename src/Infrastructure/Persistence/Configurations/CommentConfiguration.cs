using MeowStory.Domain.Entities;
using MeowStory.Infrastructure.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeowStory.Infrastructure.Persistence.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {

            builder.HasKey(c => c.Id);
            builder.HasOne(c => c.Story).WithMany().HasForeignKey(c => c.StoryId);
            builder.HasOne(c => c.User).WithMany().HasForeignKey(c => c.UserId);
            builder.Ignore(e => e.DomainEvents);
            builder.ToTable("Comments");

        }
    }
}
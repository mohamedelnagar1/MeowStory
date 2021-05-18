using MeowStory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeowStory.Infrastructure.Persistence.Configurations
{
    public class StoryConfiguration : IEntityTypeConfiguration<Story>
    {
        public void Configure(EntityTypeBuilder<Story> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(t => t.Title)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(t => t.Description)
                .HasMaxLength(512)
                .IsRequired();

            builder.Ignore(e => e.DomainEvents);

            builder.HasOne(s => s.Genre).WithMany().HasForeignKey(s => s.GenreId);
            builder.HasOne(s => s.Author).WithMany().HasForeignKey(s => s.AuthorId);
            builder.HasOne(s => s.Reviewer).WithMany(a => a.StoriesReviewed).HasForeignKey(s => s.ReviewerId);
        }
    }
}
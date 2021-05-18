using MeowStory.Domain.Entities;
using MeowStory.Infrastructure.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeowStory.Infrastructure.Persistence.Configurations
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(u => u.FirstName).HasMaxLength(32).IsRequired();
            builder.Property(u => u.LastName).HasMaxLength(32).IsRequired();
            builder.ToTable("UserProfiles");

        }
    }
}
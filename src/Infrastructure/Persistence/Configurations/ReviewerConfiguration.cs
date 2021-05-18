using MeowStory.Domain.Entities;
using MeowStory.Infrastructure.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeowStory.Infrastructure.Persistence.Configurations
{
    public class ReviewerConfiguration : IEntityTypeConfiguration<Reviewer>
    {
        public void Configure(EntityTypeBuilder<Reviewer> builder)
        {

            builder.ToTable("Reviewers");

        }
    }
}
using FluentAssertions;
using MeowStory.Application.Common.Exceptions;
using MeowStory.Application.Stories.Commands.CreateStory;
using MeowStory.Domain.Entities;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace MeowStory.Application.IntegrationTests.Stories.Commands
{
    using static Testing;
    public class CreateStoryTests
    {
        [Test]
        public void ShouldRequireTitleDescription()
        {
            var command = new CreateStoryCommand();
            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<ValidationException>();

        }

        [Test]
        public void ShouldNotAcceptTitleLongerThan64Chars()
        {
            var command = new CreateStoryCommand()
            {
                Title = new string('A', 70),
                Description = "some desc"
            };

            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public void ShouldNotAcceptDescLongerThan512Chars()
        {
            var command = new CreateStoryCommand()
            {
                Title = "some title",
                Description = new string('A', 600)
            };
            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<ValidationException>();

        }


        [Test]
        public async Task ShouldCreateStory()
        {
            var authorId = await RunAsAuthorAsync();
            var command = new CreateStoryCommand
            {
                Title = "New Story",
                Description = "New Story Description",

            };

            var storyId = await SendAsync(command);


            var story = await FindAsync<Story>(storyId);

            story.Should().NotBeNull();
            story.Title.Should().Be(command.Title);
            story.AuthorId.Should().Be(authorId);
            story.CreatedBy.Should().Be(authorId);
            story.DateCreated.Should().BeWithin(new TimeSpan(0,0,5));
            story.DateModified.Should().BeNull();
            story.DateReviewed.Should().BeNull();
            story.DateSubmitted.Should().BeNull();
            story.Content.Should().BeNull();
            story.PhotoPath.Should().BeNull();
            story.ThumbnailPath.Should().BeNull();
            story.ReviewerId.Should().BeNull();
        }
    }
}

using FluentAssertions;
using MeowStory.Application.Common.Exceptions;
using MeowStory.Application.Stories.Commands.CreateStory;
using MeowStory.Application.Stories.Commands.UpdateStory;
using MeowStory.Domain.Entities;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace MeowStory.Application.IntegrationTests.Stories.Commands
{
    using static Testing;
    public class UpdateStoryTests
    {
        [Test]
        public void ShouldRequireValidId()
        {
            var command = new UpdateStoryCommand()
            {
                Id = "abc",
                Title = "some title",
                Description = "some desc",
                Content = "some content",
                GenreId = "some genre"
            };


            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<NotFoundException>();

        }

        [Test]
        public void ShouldRequireTitleDescription()
        {
            var command = new UpdateStoryCommand();
            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<ValidationException>();

        }

        [Test]
        public void ShouldNotAcceptTitleLongerThan64Chars()
        {
            var command = new UpdateStoryCommand()
            {
                Title = new string('A', 70),
                Description = "some desc"
            };

            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public void ShouldNotAcceptDescLongerThan512Chars()
        {
            var command = new UpdateStoryCommand()
            {
                Title = "some title",
                Description = new string('A', 600)
            };
            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<ValidationException>();

        }


        [Test]
        public async Task ShouldUpdateStory()
        {
            var authorId = await RunAsAuthorAsync();
            var command = new CreateStoryCommand
            {
                Title = "New Story",
                Description = "New Story Description",
            };

            var storyId = await SendAsync(command);

            var storyBefore = await FindAsync<Story>(storyId);


            var commandUpdate = new UpdateStoryCommand()
            {
                Id = storyId,
                Title = "Edited Title",
                Description = "Edited Desc",
                Content = "some content"
            };
            await SendAsync(commandUpdate);


            var storyAfter = await FindAsync<Story>(storyId);
            storyAfter.Title.Should().Be(commandUpdate.Title);;
            storyAfter.AuthorId.Should().Be(authorId);
            storyAfter.AuthorId.Should().Be(storyBefore.AuthorId);
            storyAfter.CreatedBy.Should().Be(storyBefore.AuthorId);
            storyAfter.ModifiedBy.Should().Be(storyBefore.AuthorId);
            storyAfter.DateCreated.Should().BeWithin(new TimeSpan(0, 0, 5));
            storyAfter.DateModified.Should().BeWithin(new TimeSpan(0, 0, 5));
            storyAfter.DateReviewed.Should().BeNull();
            storyAfter.DateSubmitted.Should().BeNull();
            storyAfter.Content.Should().Be(commandUpdate.Content);
            storyAfter.PhotoPath.Should().BeNull();
            storyAfter.ThumbnailPath.Should().BeNull();
            storyAfter.ReviewerId.Should().BeNull();
        }
    }
}

using FluentAssertions;
using MeowStory.Application.Common.Exceptions;
using MeowStory.Application.Stories.Commands.CreateStory;
using MeowStory.Application.Stories.Commands.UpdateStory;
using MeowStory.Application.Stories.Commands.ReviewStory;
using MeowStory.Domain.Entities;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using MeowStory.Application.Stories.Commands.SubmitStory;

namespace MeowStory.Application.IntegrationTests.Stories.Commands
{
    using static Testing;
    public class SubmitStoryTests
    {

        [Test]
        public async Task CantSubmitWithoutMinimalValues()
        {
            await RunAsAuthorAsync();

            var command = new CreateStoryCommand
            {
                Title = "New Story",
                Description = "New Story Description",
            };

            var storyId = await SendAsync(command);


            var commandSubmit = new SubmitStoryCommand()
            {
                Id = storyId,
            };

            FluentActions.Invoking(() => SendAsync(commandSubmit)).Should().Throw<InvalidOperationException>();
        }

        [Test]
        public async Task ShouldSubmitStory()
        {
            string authorId, reviewerId;
            authorId = await RunAsAuthorAsync();

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

            //Switch to reviewer to add reviewer record
            reviewerId = await RunAsReviewerAsync();

            //Switch to author again
            await RunAsAuthorAsync();

            var commandSubmit = new SubmitStoryCommand()
            {
                Id = storyId
            };

            await SendAsync(commandSubmit);

            var storyAfterSubmit = await FindAsync<Story>(storyId);

            storyAfterSubmit.AuthorId.Should().Be(authorId);
            storyAfterSubmit.AuthorId.Should().Be(storyBefore.AuthorId);
            storyAfterSubmit.CreatedBy.Should().Be(storyBefore.AuthorId);
            storyAfterSubmit.DateModified.Should().BeWithin(new TimeSpan(0, 0, 5));
            storyAfterSubmit.DateReviewed.Should().BeNull();
            storyAfterSubmit.DateSubmitted.Should().NotBeNull();
            storyAfterSubmit.ReviewerId.Should().Be(reviewerId);
        }
    }

}

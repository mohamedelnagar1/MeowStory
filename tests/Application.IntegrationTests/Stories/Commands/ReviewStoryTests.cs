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
    public class ReviewStoryTests
    {
        [Test]
        public void ShouldRequireValidId()
        {
            var command = new ReviewStoryCommand()
            {
                Id = "abc",
                IsApproved = true
            };

            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldRequireReasonWithRejection()
        {
            var command = new CreateStoryCommand
            {
                Title = "New Story",
                Description = "New Story Description",
            };

            var storyId = await SendAsync(command);


            var commandReview = new ReviewStoryCommand()
            {
                Id = storyId,
                IsApproved = false
            };

            FluentActions.Invoking(() => SendAsync(commandReview)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldNotRequireReasonWithApproval()
        {
            var command = new CreateStoryCommand
            {
                Title = "New Story",
                Description = "New Story Description",
            };

            var storyId = await SendAsync(command);


            var commandReview = new ReviewStoryCommand()
            {
                Id = storyId,
                IsApproved = true,
                RejectionReason = "abc"
            };

            FluentActions.Invoking(() => SendAsync(commandReview)).Should().Throw<ValidationException>();
        }


        [Test]
        public async Task ShouldReviewStory()
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
            authorId = await RunAsAuthorAsync();

            var commandSubmit = new SubmitStoryCommand()
            {
                Id = storyId
            };

            await SendAsync(commandSubmit);


            //Switch to reviewer again
            reviewerId = await RunAsReviewerAsync();
            var commandReview = new ReviewStoryCommand()
            {
                Id = storyId,
                IsApproved = true
            };

            await SendAsync(commandReview);


            var storyAfterReview = await FindAsync<Story>(storyId);


            storyAfterReview.AuthorId.Should().Be(authorId);
            storyAfterReview.AuthorId.Should().Be(storyBefore.AuthorId);
            storyAfterReview.CreatedBy.Should().Be(storyBefore.AuthorId);
            storyAfterReview.ModifiedBy.Should().Be(reviewerId);
            storyAfterReview.DateModified.Should().BeWithin(new TimeSpan(0, 0, 5));
            storyAfterReview.DateReviewed.Should().BeWithin(new TimeSpan(0, 0, 5));
            storyAfterReview.DateSubmitted.Should().NotBeNull();
            storyAfterReview.ReviewerId.Should().Be(reviewerId);
        }
    }
}

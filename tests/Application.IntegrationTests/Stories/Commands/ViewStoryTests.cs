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
using MeowStory.Application.Stories.Commands.ViewStory;

namespace MeowStory.Application.IntegrationTests.Stories.Commands
{
    using static Testing;
    public class ViewStoryTests
    {
        [Test]
        public async Task ShouldNotViewNotSubmittedStory()
        {
            string authorId = authorId = await RunAsAuthorAsync();

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


            var commandView = new ViewStoryCommand()
            {
                Id = storyId
            };


            FluentActions.Invoking(() => SendAsync(commandView)).Should().Throw<InvalidOperationException>();
        }

        [Test]
        public async Task ShouldNotViewNotReviewedStory()
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


            var commandView = new ViewStoryCommand()
            {
                Id = storyId
            };

            FluentActions.Invoking(() => SendAsync(commandView)).Should().Throw<InvalidOperationException>();
        }

        [Test]
        public async Task ShouldNotViewRejectedStory()
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
                IsApproved = false,
                RejectionReason = "some reason"
            };

            await SendAsync(commandReview);

            var commandView = new ViewStoryCommand()
            {
                Id = storyId
            };

            FluentActions.Invoking(() => SendAsync(commandView)).Should().Throw<InvalidOperationException>();
        }


        [Test]
        public async Task ShouldStoryViewsIncrementByOne()
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

            for (int i = 0; i < 50; i++)
            {
                var commandView = new ViewStoryCommand()
                {
                    Id = storyId
                };

                await SendAsync(commandView);

            }

            var storyAfterReview = await FindAsync<Story>(storyId);


            storyAfterReview.Views.Should().Be(50);
        }
    }
}

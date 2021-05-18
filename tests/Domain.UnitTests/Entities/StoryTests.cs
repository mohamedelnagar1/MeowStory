using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;
using MeowStory.Domain.Entities;

namespace MeowStory.Domain.UnitTests.Entities
{
    public class StoryTests
    {
        [Test]
        [TestCase(null, null, null)]
        [TestCase("", "", "")]
        [TestCase("", "some desc", "some auth")]
        [TestCase("some title", "", "some auth")]
        [TestCase("some title", "some desc", "")]
        public void CantCreateStoryWithEmptyOrNullArgs(string title, string desc, string author)
        {
            FluentActions.Invoking(() => new Story(title, desc, author)).Should().Throw<InvalidOperationException>();
            // Assert.Throws<InvalidOperationException>(() => new Story(title, desc, author));
        }


        [Test]
        public void DefaultVotesToZero()
        {
            var story = new Story("some title", "some desc", "some auth");

            story.Votes.Should().Equals(0);
        }

        [Test]
        public void DefaultViewsToZero()
        {
            var story = new Story("some title", "some desc", "some auth");

            story.Views.Should().Equals(0);
        }

        [Test]
        public void ShouldIncrementVotesByOne()
        {
            var story = new Story("some title", "some desc", "some auth");

            story.SetContent("some content");
            story.Submit(DateTime.Now);
            story.Approve(DateTime.Now);

            story.IncrementVotes();
            story.IncrementVotes();
            story.Votes.Should().Equals(2);

        }

        [Test]
        public void ShouldIncrementViewsByOne()
        {
            var story = new Story("some title", "some desc", "some auth");
            story.SetContent("some content");

            story.Submit(DateTime.Now);
            story.Approve(DateTime.Now);

            story.IncrementViews();
            story.IncrementViews();
            story.Views.Should().Equals(2);

        }


        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void CantModifyTitleToEmptyOrNull(string title)
        {
            var story = new Story("some title", "some desc", "some auth");

            FluentActions.Invoking(() => story.SetTitle(title)).Should().Throw<InvalidOperationException>();

        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void CantModifyDescriptionToEmptyOrNull(string desc)
        {
            var story = new Story("some title", "some desc", "some auth");

            FluentActions.Invoking(() => story.SetDescription(desc)).Should().Throw<InvalidOperationException>();

        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void CantModifyAuthorToEmptyOrNull(string title)
        {
            var story = new Story("some title", "some desc", "some auth");

            FluentActions.Invoking(() => story.SetAuthor(title)).Should().Throw<InvalidOperationException>();

        }


        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void MustHaveRejectionReason(string reason)
        {
            var story = new Story("some title", "some desc", "some auth");

            FluentActions.Invoking(() => story.Reject(DateTime.Now, reason)).Should().Throw<InvalidOperationException>();

        }


        [Test]
        public void ApproveShouldModifyRejectionReasonToNull()
        {
            var story = new Story("some title", "some desc", "some auth");
            
            story.SetContent("some content");
            story.Submit(DateTime.Now);
            story.Reject(DateTime.Now, "some reason");
            story.Approve(DateTime.Now);
            story.RejectionReason.Should().BeNull();

        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void CantAcceptNullOrEmptyContent(string content)
        {
            var story = new Story("some title", "some desc", "some auth");

            FluentActions.Invoking(() => story.SetContent(content)).Should().Throw<InvalidOperationException>();

        }

        [Test]
        public void CantSubmitStoryWithNullOrEmptyContent()
        {
            var story = new Story("some title", "some desc", "some auth");


            FluentActions.Invoking(() => story.Submit(DateTime.Now)).Should().Throw<InvalidOperationException>();

        }

        [Test]
        public void CantApproveStoryWithNullOrEmptyContent()
        {
            var story = new Story("some title", "some desc", "some auth");

            FluentActions.Invoking(() => story.Approve(DateTime.Now)).Should().Throw<InvalidOperationException>();

        }

    }
}
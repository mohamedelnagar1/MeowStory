using MeowStory.Domain.Common;
using MeowStory.Domain.Enums;
using MeowStory.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowStory.Domain.Entities
{
    public class Story : BaseEntity, IHasDomainEvent
    {
        public string Title { get; private set; }
        public Author Author { get; private set; }
        public Reviewer Reviewer { get; private set; }
        public string AuthorId { get; private set; }
        public string ReviewerId { get; private set; }
        public Genre Genre { get; private set; }
        public string GenreId { get; set; }
        public string Description { get; private set; }
        public string PhotoPath { get; set; }
        public string ThumbnailPath { get; set; }
        public int Votes { get; private set; }
        public int Views { get; private set; }
        public string Content { get; private set; }
        public ReviewStatus? ReviewStatus { get; private set; }
        public string RejectionReason { get; private set; }
        public virtual IEnumerable<Comment> Comments { get; private set; }
        public DateTime? DateReviewed { get; private set; }
        public DateTime? DateSubmitted { get; private set; }
        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();

        public DateTime? DatePublished
        {
            get
            {
                if (ReviewStatus == Enums.ReviewStatus.Approved)
                {
                    return DateReviewed;
                }
                else
                {
                    return null;
                }
            }
        }
        private Story()
        {
            this.Votes = 0;
            this.Views = 0;
        }

        public Story(string title, string description, string authorId) : this()
        {
            this.SetTitle(title);
            this.SetDescription(description);
            this.SetAuthor(authorId);
        }

        public void SetContent(string content)
        {
            if (string.IsNullOrEmpty(content))
                throw new InvalidOperationException();

            this.Content = content;
        }

        public void SetTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new InvalidOperationException();
            }

            this.Title = title;
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                throw new InvalidOperationException();
            }

            this.Description = description;
        }

        public void SetAuthor(string authorId)
        {
            if (string.IsNullOrEmpty(authorId))
            {
                throw new InvalidOperationException();
            }
            this.AuthorId = authorId;
        }

        public void AssignReviewer(string reviewerId)
        {
            if (string.IsNullOrEmpty(reviewerId))
                throw new InvalidOperationException();

            this.ReviewerId = reviewerId;
        }

        public void Submit(DateTime dateTime)
        {
            if (string.IsNullOrEmpty(this.Title) || string.IsNullOrEmpty(this.Description)
                || string.IsNullOrEmpty(this.Content))
            {
                throw new InvalidOperationException();
            }

            this.DateSubmitted = dateTime;
        }

        public void IncrementViews()
        {
            if (!(this.DateSubmitted.HasValue && this.DateReviewed.HasValue && this.ReviewStatus == Enums.ReviewStatus.Approved))
            {
                throw new InvalidOperationException();
            }

            this.Views += 1;
        }

        public void IncrementVotes()
        {
            if (!(this.DateSubmitted.HasValue && this.DateReviewed.HasValue && this.ReviewStatus == Enums.ReviewStatus.Approved))
            {
                throw new InvalidOperationException();
            }

            this.Votes += 1;
        }

        public void Approve(DateTime dateTime)
        {
            if (string.IsNullOrEmpty(this.Title) || string.IsNullOrEmpty(this.Description)
                || string.IsNullOrEmpty(this.Content)
                || !this.DateSubmitted.HasValue)
            {
                throw new InvalidOperationException();
            }

            this.DateReviewed = dateTime;
            this.ReviewStatus = Enums.ReviewStatus.Approved;
            this.RejectionReason = null;
        }

        public void Reject(DateTime dateTime, string rejectionReason)
        {
            if (string.IsNullOrEmpty(this.Title) || string.IsNullOrEmpty(this.Description)
                || string.IsNullOrEmpty(this.Content)
                || !this.DateSubmitted.HasValue || string.IsNullOrEmpty(rejectionReason))
            {
                throw new InvalidOperationException();
            }
            this.DateReviewed = dateTime;
            this.ReviewStatus = Enums.ReviewStatus.Rejected;
            this.RejectionReason = rejectionReason;
        }

    }
}
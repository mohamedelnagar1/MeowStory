using System.Collections.Generic;

namespace MeowStory.Domain.Entities
{
    public class Reviewer : UserProfile
    {

        public Reviewer(string id, string firstName, string lastName) : base(id, firstName, lastName)
        {
        }

        public IEnumerable<Story> StoriesReviewed { get; set; }

    }
}
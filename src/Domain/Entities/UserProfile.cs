using MeowStory.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowStory.Domain.Entities
{
    public class UserProfile : BaseEntity
    {
        protected UserProfile() { }

        public UserProfile(string id, string firstName, string lastName)
        {
            this.SetId(id);
            this.SetFirstName(firstName);
            this.SetLastName(lastName);
        }


        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string PhotoPath { get; set; }

        public string FullName { get => $"{FirstName} {LastName}"; }
        public void SetId(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new InvalidOperationException();

            this.Id = id;
        }

        public void SetFirstName(string firstName)
        {
            if (string.IsNullOrEmpty(firstName))
                throw new InvalidOperationException();

            this.FirstName = firstName;
        }

        public void SetLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
                throw new InvalidOperationException();

            this.LastName = lastName;
        }
    }
}

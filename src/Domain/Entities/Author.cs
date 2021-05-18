using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowStory.Domain.Entities
{
    public class Author : UserProfile
    {
        public Author(string id, string firstName, string lastName) : base(id, firstName, lastName)
        {
        }

        //public virtual IEnumerable<Story> Stories { get; private set; }
    }
}
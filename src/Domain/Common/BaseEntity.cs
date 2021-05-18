using System;

namespace MeowStory.Domain.Common
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            if (string.IsNullOrEmpty(this.Id))
            {
                this.Id = Guid.NewGuid().ToString();//TODO: Use Abstract ID Provider 
            }
        }

        public string Id { get; protected set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string ModifiedBy { get; set; }
    }
}
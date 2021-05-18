using System;

namespace MeowStory.Application.Authentication
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class AuthorizeAttribute : Attribute
    {
        public AuthorizeAttribute() { }

        public string Roles { get; set; }

    }
}

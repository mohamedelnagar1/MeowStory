using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowStory.Application.Authentication
{
    public class AuthenticateResponse
    {
        public bool Succeeded { get; private set; }
        public TokenResponse TokenResponse { get; private set; }

        private AuthenticateResponse() { }
        public AuthenticateResponse(bool succeeded)
        {
            this.Succeeded = succeeded;
        }
        public AuthenticateResponse(bool succeeded, TokenResponse tokenResponse) : this(succeeded)
        {
            if (this.Succeeded && tokenResponse == null)
                throw new InvalidOperationException();


            this.TokenResponse = tokenResponse;
        }

    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace PetProjectStore.Api.Exceptions
{
    public class LogInException : Exception
    {
        public IEnumerable<IdentityError> IdentityErrors { get; private set; }

        public LogInException(string message) : base (message)
        {
        }

        public LogInException(IEnumerable<IdentityError> identityErrors)
        {
            IdentityErrors = identityErrors;
        }
    }
}

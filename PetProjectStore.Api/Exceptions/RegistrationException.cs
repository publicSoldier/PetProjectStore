using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace PetProjectStore.Api.Exceptions
{
    public class RegistrationException : Exception
    {
        public IEnumerable<IdentityError> IdentityErrors { get; private set; }

        public RegistrationException(IEnumerable<IdentityError> identityErrors)
        {
            IdentityErrors = identityErrors;
        }
    }
}

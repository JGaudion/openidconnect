
using OpenIDConnect.Core.Services;
using System;

namespace OpenIDConnect.IdentityServer.Factories
{
    internal class DefaultUserServiceFactory
    {
        public IUserAuthenticationService Create(UserStoreType userStoreType)
        {
            switch (userStoreType)
            {
                case UserStoreType.AspNetIdentity:
                    throw new NotImplementedException();

                case UserStoreType.MembershipReboot:
                    throw new NotImplementedException();

                case UserStoreType.Adlds:
                    throw new NotImplementedException();

                default:
                    throw new InvalidOperationException("Invalid user store type specified");
            }
        }
    }
}
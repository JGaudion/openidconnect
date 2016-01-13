using System;
using OpenIDConnect.IdentityServer3.AdLds.Contexts;
using OpenIDConnect.IdentityServer3.AdLds.Models;

namespace OpenIDConnect.IdentityServer3.AdLds.Factories
{
    public class AdLdsDirectoryContextFactory : IDirectoryContextFactory
    {
        public IDirectoryContext CreateDirectoryContext(DirectoryConnectionConfig config)
        {
            return new AdLdsDirectoryContext(config);
        }
    }
}

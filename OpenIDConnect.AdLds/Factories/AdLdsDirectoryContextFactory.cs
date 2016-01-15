using System;
using OpenIDConnect.AdLds.Contexts;
using OpenIDConnect.AdLds.Models;

namespace OpenIDConnect.AdLds.Factories
{
    public class AdLdsDirectoryContextFactory : IDirectoryContextFactory
    {
        public IDirectoryContext CreateDirectoryContext(DirectoryConnectionConfig config)
        {
            return new AdLdsDirectoryContext(config);
        }
    }
}

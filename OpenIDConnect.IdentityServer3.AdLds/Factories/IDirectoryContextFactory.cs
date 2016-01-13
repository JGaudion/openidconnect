using OpenIDConnect.IdentityServer3.AdLds.Contexts;
using OpenIDConnect.IdentityServer3.AdLds.Models;

namespace OpenIDConnect.IdentityServer3.AdLds.Factories
{
    public interface IDirectoryContextFactory
    {
        IDirectoryContext CreateDirectoryContext(DirectoryConnectionConfig config);
    }
}

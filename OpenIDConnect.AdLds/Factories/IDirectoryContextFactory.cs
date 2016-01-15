using OpenIDConnect.AdLds.Contexts;
using OpenIDConnect.AdLds.Models;

namespace OpenIDConnect.AdLds.Factories
{
    public interface IDirectoryContextFactory
    {
        IDirectoryContext CreateDirectoryContext(DirectoryConnectionConfig config);
    }
}

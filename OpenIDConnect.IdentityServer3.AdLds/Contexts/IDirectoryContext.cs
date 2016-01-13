using OpenIDConnect.IdentityServer3.AdLds.Models;
using System.DirectoryServices;
using System.Threading.Tasks;

namespace OpenIDConnect.IdentityServer3.AdLds.Contexts
{
    public interface IDirectoryContext
    {
        Task<SearchResultCollection> FindAllAsync(DirectoryQuery query);
    }
}

using OpenIDConnect.AdLds.Models;
using System.DirectoryServices;
using System.Threading.Tasks;

namespace OpenIDConnect.AdLds.Contexts
{
    public interface IDirectoryContext
    {
        Task<AdLdsUser> ValidateCredentialsAsync(string username, string password);

        Task<AdLdsUser> FindUserByNameAsync(string username);
    }
}

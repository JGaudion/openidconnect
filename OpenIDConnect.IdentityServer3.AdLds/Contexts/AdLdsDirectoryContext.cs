using OpenIDConnect.IdentityServer3.AdLds.Models;
using System;
using System.DirectoryServices;
using System.Threading.Tasks;

namespace OpenIDConnect.IdentityServer3.AdLds.Contexts
{
    public class AdLdsDirectoryContext : IDirectoryContext
    {
        private readonly DirectoryEntry directoryEntry;

        public AdLdsDirectoryContext(DirectoryConnectionConfig config)
        {
            string path = $"{config.Prefix}{config.ServerName}:{config.Port}/{config.DbString}";

            try
            {
                this.directoryEntry = new DirectoryEntry(path);
                this.directoryEntry.RefreshCache();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: directory bind for path {path} failed. Exception: {e}");
                throw;
            }
        }

        public Task<SearchResultCollection> FindAllAsync(DirectoryQuery query)
        {
            return Task.Run(() =>
            {
                try
                {
                    var searcher = new DirectorySearcher(this.directoryEntry);
                    searcher.Filter = query.Filter;
                    searcher.SearchScope = SearchScope.Subtree;
                    return searcher.FindAll();
                }
                    catch (Exception e)
                {
                    Console.WriteLine($"Error: query \"{query}\" failed with exception {e}");
                    throw;
                }
            });
        }
    }
}

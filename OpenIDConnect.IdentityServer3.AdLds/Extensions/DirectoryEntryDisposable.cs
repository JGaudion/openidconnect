using System;
using System.DirectoryServices;

namespace OpenIDConnect.IdentityServer3.AdLds.Extensions
{
    internal class DirectoryEntryDisposable : IDisposable
    {
        public DirectoryEntryDisposable(DirectoryEntry entry)
        {
            this.Entry = entry;
        }

        public DirectoryEntry Entry { get; }

        public void Dispose()
        {
            this.Entry.Close();
        }
    }

    internal static class DirectoryEntryExtensions
    {
        public static DirectoryEntryDisposable ToDisposable(this DirectoryEntry entry)
        {
            return new DirectoryEntryDisposable(entry);
        }
    }
        
}

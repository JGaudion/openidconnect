using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace OpenIDConnect.Core
{
    public class Cert
    {
        public static X509Certificate2 Load(StoreName storeName, StoreLocation location, string thumbprint)
        {
            X509Store store = null;

            try
            {
                store = new X509Store(storeName, location);
                store.Open(OpenFlags.ReadOnly);

                return store.Certificates
                    .OfType<X509Certificate2>()
                    .FirstOrDefault(c => string.Compare(c.Thumbprint, thumbprint, StringComparison.OrdinalIgnoreCase) == 0);
            }
            finally
            {
                if (store != null)
                {
                    store.Close();
                }
            }
        }

        public static X509Certificate2 Load(Assembly assembly, string folder, string fileName, string password)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            if (string.IsNullOrWhiteSpace(folder))
            {
                throw new ArgumentNullException("folder");
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            using (var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{folder}.{fileName}"))
            {
                return new X509Certificate2(ReadStream(stream), password);
            }
        }

        private static byte[] ReadStream(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
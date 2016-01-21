using System;
using Nancy;
using Nancy.Conventions;
using System.IO;

namespace OpenIDConnect.Clients.Angular14.Bootstrap
{
    public class CustomNancyBootstrapper : DefaultNancyBootstrapper
    {
        protected override IRootPathProvider RootPathProvider
        {
            get { return new CustomRootPathProvider(new DefaultRootPathProvider()); }
        }
    }

    internal class CustomRootPathProvider : IRootPathProvider
    {
        private readonly IRootPathProvider provider;

        public CustomRootPathProvider(IRootPathProvider provider)
        {
            this.provider = provider;
        }

        public string GetRootPath()
        {
            var path = Path.Combine(this.provider.GetRootPath(), "content/dist/");
            return path;
        }
    }   
}
using OpenIDConnect.Core;

namespace OpenIDConnect.AdLds.Models
{
    public class DirectoryConnectionConfig
    {
        public DirectoryConnectionConfig(IConfigurationService configService)
        {
            this.ServerName = configService.GetSetting<string>("adLds:serverName", string.Empty);
            this.Port = configService.GetSetting<string>("adLds:port", string.Empty);
            this.Prefix = configService.GetSetting<string>("adLds:prefix", string.Empty);
            this.Container = configService.GetSetting<string>("adLds:container", string.Empty);
        }

        public string ServerName { get; }

        public string Port { get; }

        public string Prefix { get; }

        public string Container { get; }

        public string Path
        {
            get
            {
                return $"{this.Prefix}{this.ServerName}:{this.Port}/{this.Container}";
            }
        }
    }
}

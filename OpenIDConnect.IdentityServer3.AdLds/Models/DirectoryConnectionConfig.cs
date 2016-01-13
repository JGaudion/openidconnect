namespace OpenIDConnect.IdentityServer3.AdLds.Models
{
    public class DirectoryConnectionConfig
    {
        public DirectoryConnectionConfig(
            string serverName,
            string port,
            string prefix,
            string container)
        {
            this.ServerName = serverName;
            this.Port = port;
            this.Prefix = prefix;
            this.Container = container;
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

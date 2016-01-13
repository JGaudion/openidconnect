namespace OpenIDConnect.IdentityServer3.AdLds.Models
{
    public class DirectoryConnectionConfig
    {
        public DirectoryConnectionConfig(
            string serverName,
            string port,
            string prefix,
            string dbString)
        {
            this.ServerName = serverName;
            this.Port = port;
            this.Prefix = prefix;
            this.DbString = dbString;
        }

        public string ServerName { get; }

        public string Port { get; }

        public string Prefix { get; }

        public string DbString { get; }
    }
}

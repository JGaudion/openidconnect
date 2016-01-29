namespace OpenIDConnect.Users.Api.IntegrationTests.Tests
{
    using Microsoft.Extensions.Logging;

    internal class NoopLoggerFactory : ILoggerFactory
    {
        public void Dispose()
        {            
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new NoopLogger();
        }

        public void AddProvider(ILoggerProvider provider)
        {            
        }

        public LogLevel MinimumLevel { get; set; }
    }
}
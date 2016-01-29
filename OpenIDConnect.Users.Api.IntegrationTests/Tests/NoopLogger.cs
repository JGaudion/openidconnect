namespace OpenIDConnect.Users.Api.IntegrationTests.Tests
{
    using System;

    using Microsoft.Extensions.Logging;

    internal class NoopLogger : ILogger
    {
        public void Log(LogLevel logLevel, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
        {            
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }

        public IDisposable BeginScopeImpl(object state)
        {
            return null;
        }
    }
}
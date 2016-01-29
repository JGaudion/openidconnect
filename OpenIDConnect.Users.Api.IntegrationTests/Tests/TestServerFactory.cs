namespace OpenIDConnect.Users.Api.IntegrationTests.Tests
{
    using Microsoft.AspNet.Hosting;
    using Microsoft.AspNet.TestHost;
    using Microsoft.Extensions.DependencyInjection;

    public class TestServerFactory
    {
        public TestServer Create()
        {
            var bootstrap = new UsersApiBootstrap();
            var configuration = bootstrap.GetConfiguration();

            return TestServer.Create(
                app =>
                    {
                        var env = app.ApplicationServices.GetRequiredService<IHostingEnvironment>();
                        bootstrap.Configure(app, env, new NoopLoggerFactory(), configuration);
                    }, services => bootstrap.ConfigureServices(services, configuration));
        }
    }
}
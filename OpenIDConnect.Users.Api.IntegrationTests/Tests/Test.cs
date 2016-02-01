namespace OpenIDConnect.Users.Api.IntegrationTests.Tests
{
    using System.Net;

    using Ploeh.AutoFixture.Xunit2;

    using Xunit;

    public class Test
    {        
        [Theory, AutoData]
        public void UsersWithValidQueryStringShouldReturn200(TestServerFactory testServerFactory)
        {
            using (var testServer = testServerFactory.Create())
            {
                using (var client = testServer.CreateClient())
                {
                    var response = client.GetAsync("/api/users?page=1&pageSize=25").Result;
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                }                    
            }
        }
    }
}
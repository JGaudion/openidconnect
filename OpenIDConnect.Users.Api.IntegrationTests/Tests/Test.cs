
namespace OpenIDConnect.Users.Api.IntegrationTests.Tests
{
    using System.Net;

    using Xunit;

    public class Test
    {        
        [Fact]
        public void UsersWithValidQueryStringShouldReturn200()
        {
            using (var testServer = new TestServerFactory().Create())
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

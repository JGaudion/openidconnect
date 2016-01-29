
namespace OpenIDConnect.Users.Api.IntegrationTests.Tests
{
    using System.Net;

    using Ploeh.AutoFixture.Xunit;

    using Xunit;

    public class Test
    {        
        [Theory, AutoData]
        public void UsersWithValidQueryStringShouldReturn200(TestServerFactory testServerFactory)
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

        [Theory]
        [InlineAutoData(2)]
        public void Theory()
        {
            Assert.Equal(5, 5);
        }

        [Fact]
        public void Test2()
        {
            Assert.True(true);
        }
    }
}

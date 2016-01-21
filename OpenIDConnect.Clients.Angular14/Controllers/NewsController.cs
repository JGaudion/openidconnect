using OpenIDConnect.Clients.Angular14.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace OpenIDConnect.Clients.Angular14.Controllers
{
    public class NewsController : ApiController
    {
        [HttpGet]
        [Route("api/news")]
        public IHttpActionResult GetNews()
        {
            var newsArticles = new List<NewsArticleApiModel>()
            {
                new NewsArticleApiModel { Id = 1, Title = "News article 1", Body = "This is some sample news" },
                new NewsArticleApiModel { Id = 2, Title = "News article 2", Body = "This is some more sample news" }
            };
            
            return this.Ok(newsArticles);
        }
    }
}
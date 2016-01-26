using System.Collections.Generic;
using System.Web.Http;
using OpenIDConnect.Clients.AngularMaterial.Models;

namespace OpenIDConnect.Clients.AngularMaterial.Controllers
{
    public class CartoonController : ApiController
    {

        [HttpGet]
        [Route("api/cartoons")]
        public IHttpActionResult GetCartoons()
        {
            var cartoons = new List<Cartoon>()
            {
                new Cartoon() {Title = "Thundercats"},
                new Cartoon() {Title = "Transformers"},
                new Cartoon() {Title = "Teenage Mutant Hero Turtles"},
                new Cartoon() {Title = "Animals of Farthing Wood"}
            };

            return Ok(cartoons);
        }
    }
}
using Microsoft.AspNet.Mvc;
using OpenIDConnect.Authorization.Api.Models;
using OpenIDConnect.Authorization.Domain.Repositories;
using OpenIDConnect.Core.Api.Models;
using OpenIDConnect.Core.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OpenIDConnect.Authorization.Api.Controllers
{
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly IUsersRepository usersRepository;

        public UsersController(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingApiModel paging)
        {
            var result = await this.usersRepository.GetUsers(new Paging(paging.Page, paging.PageSize));

            var apiResult = new PagingResultApiModel<UserApiModel>
            {
                Items = result.Items.Select(u => new UserApiModel { Id = u.Id, Username = u.Username }),
                Paging = PageDetailsApiModel.FromDomain(result.Paging)
            };

            return this.Ok(apiResult);
        }

        [HttpGet("{userId}/groups")]
        public async Task<IActionResult> GetGroups(string userId)
        {
            var result = await this.usersRepository.GetUserGroups(userId);

            return this.Ok(result.Select(g => new ClientGroupApiModel { ClientId = g.ClientId, Id = g.Id }));
        }

        [HttpGet("{userId}/clients")]
        public async Task<IActionResult> GetClients(string userId)
        {
            var result = await this.usersRepository.GetUserClaims(userId);

            return this.Ok(result.Select(c => new ClaimApiModel { Type = c.Type, Value = c.Value }));
        }
    }
}

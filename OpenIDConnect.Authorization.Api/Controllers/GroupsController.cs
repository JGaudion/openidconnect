
namespace OpenIDConnect.Authorization.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Mvc;

    using OpenIDConnect.Authorization.Api.Models;
    using OpenIDConnect.Authorization.Domain.Repositories;
    using OpenIDConnect.Core.Api.Results;

    [Route("api/clients")]
    public class GroupsController : Controller
    {
        private readonly IClientGroupsRepository clientGroupsRepository;

        public GroupsController(IClientGroupsRepository clientGroupsRepository)
        {
            if (clientGroupsRepository == null)
            {
                throw new ArgumentNullException(nameof(clientGroupsRepository));
            }

            this.clientGroupsRepository = clientGroupsRepository;
        }

        [HttpGet("{clientId}/groups")]
        public async Task<IActionResult> GetClientGroups(string clientId)
        {
            var groups = await this.clientGroupsRepository.GetGroups(clientId);
            var groupApiModels = groups.Select(g => new GroupApiModel(g));
            return this.Ok(groupApiModels);
        }

        [HttpPost("{clientId}/groups")]
        public async Task<IActionResult> AddClientGroup(string clientId, [FromBody] GroupApiModel groupApiModel)
        {
            var group = groupApiModel.ToDomainModel();
            await this.clientGroupsRepository.AddGroup(clientId, group);
            return new EntityCreatedResult();
        }

        [HttpGet("{clientId}/groups/{groupId}")]
        public async Task<IActionResult> GetClientGroup(string clientId, string groupId)
        {
            var group = await this.clientGroupsRepository.GetGroup(clientId, groupId);
            if (group == null)
            {
                return this.HttpNotFound();
            }

            var groupApiModel = new GroupApiModel(group);
            return this.Ok(groupApiModel);
        }
    }
}
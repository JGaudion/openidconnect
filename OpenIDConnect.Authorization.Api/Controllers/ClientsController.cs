using Microsoft.AspNet.Mvc;
using OpenIDConnect.Authorization.Domain.Repositories;
using System;
using System.Linq;

namespace OpenIDConnect.Authorization.Api.Controllers
{
    using System.Threading.Tasks;

    using OpenIDConnect.Authorization.Api.Models;

    [Route("api/clients")]
    public class ClientsController : Controller
    {
        private readonly IClientsRepository clientsRepository;

        public ClientsController(IClientsRepository clientsRepository)
        {
            if (clientsRepository == null)
            {
                throw new ArgumentNullException(nameof(clientsRepository));
            }

            this.clientsRepository = clientsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var clients = await this.clientsRepository.GetClients();
            var clientApiModels = clients.Select(c => new ClientApiModel(c));
            return this.Ok(clientApiModels);
        }        
    }
}

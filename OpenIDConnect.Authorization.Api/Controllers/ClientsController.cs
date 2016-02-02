﻿namespace OpenIDConnect.Authorization.Api.Controllers
{    
    using Microsoft.AspNet.Mvc;
    using OpenIDConnect.Authorization.Api.Models;
    using OpenIDConnect.Authorization.Domain.Repositories;
    using OpenIDConnect.Core.Api.Results;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    
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
        public async Task<IActionResult> GetAll()
        {
            var clients = await this.clientsRepository.GetClients();
            var clientApiModels = clients.Select(c => new ClientApiModel(c));
            return this.Ok(clientApiModels);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ClientApiModel apiModel)
        {
            if (!this.ModelState.IsValid)
            {
                return new UnprocessableEntityResult();
            }

            await this.clientsRepository.Add(apiModel.ToDomainModel());
            return new EntityCreatedResult();
        }

        [HttpGet("{clientId}")]
        public async Task<IActionResult> GetClient(string clientId)
        {
            var client = await this.clientsRepository.GetClient(clientId);
            if (client == null)
            {
                return this.HttpNotFound();
            }

            var clientApiModel = new ClientApiModel(client);
            return this.Ok(clientApiModel);
        }

        [HttpPut("{clientId}")]
        public async Task<IActionResult> UpdateClient(string clientId, [FromBody] ClientApiModel apiModel)
        {
            if (!this.ModelState.IsValid)
            {
                return new UnprocessableEntityResult();
            }

            apiModel.Id = clientId;
            var client = apiModel.ToDomainModel();
            await this.clientsRepository.Update(client);
            return new EntityCreatedResult();
        }

        [HttpDelete("{clientId}")]
        public async Task<IActionResult> DeleteClient(string clientId)
        {
            await this.clientsRepository.Delete(clientId);
            return this.Ok();
        }
    }
}
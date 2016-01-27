using Microsoft.AspNet.Mvc;
using OpenIDConnect.Users.Domain;
using System;
using OpenIDConnect.Users.Api.Models;
using System.Threading.Tasks;
using OpenIDConnect.Core.Api.Models;
using System.Linq;
using OpenIDConnect.Core.Domain.Models;
using System.Collections.Generic;
using OpenIDConnect.Users.Domain.Models;

namespace OpenIDConnect.Users.Api.Controllers
{    
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly IUsersRepository usersRepository;

        public UsersController(IUsersRepository usersRepository)
        {
            if (usersRepository == null)
            {
                throw new ArgumentNullException("usersRepository");
            }

            this.usersRepository = usersRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(string username, [FromQuery] PagingApiModel pagingApiModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.HttpBadRequest();
            }

            var domainResult = (await this.usersRepository.QueryUsers(username, new Paging(pagingApiModel.Page, pagingApiModel.PageSize)));

            var result = new PagingResultApiModel<UserApiModel>
            {
                Page = domainResult.Page,
                PageSize = domainResult.PageSize,
                Count = domainResult.Count,
                Total = domainResult.Total,
                Items = domainResult.Items.Select(u => new UserApiModel { Id = u.Id })
            };

            return this.Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserCreateApiModel userApiModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.HttpBadRequest();   // TODO: unprocessible entity response
            }

            await this.usersRepository.AddUser(
                userApiModel.ToDomainModel());

            return this.Ok();       // TODO: return created response
        }
        
        [HttpGet("{username}")]
        public async Task<IActionResult> Get(string username)
        {
            var user = await this.usersRepository.GetUserByName(username);
            if (user == null)
            {
                return this.HttpNotFound();
            }

            var userApiModel = new UserApiModel { Id = user.Id };
            return this.Ok(userApiModel);
        }
        
        [HttpPut("{username}")]
        public void Put(string username, [FromBody]UpdateUserApiModel userApiModel)
        {
            throw new NotImplementedException();
        }
        
        [HttpDelete("{username}")]
        public async Task<IActionResult> Delete(string username)
        {
            await this.usersRepository.DeleteUser(username);
            return this.Ok();
        }

        [HttpPost("{username}/authenticate")]
        public async Task<IActionResult> Authenticate(string username, string password)
        {
            var passwordMatches = 
                await this.usersRepository.Authenticate(username, password);

            if (passwordMatches)
            {
                return this.Ok();
            }

            return this.HttpBadRequest();
        }
    }
}
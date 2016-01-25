using Microsoft.AspNet.Mvc;
using OpenIDConnect.Users.Domain;
using System;
using OpenIDConnect.Users.Api.Models;
using System.Threading.Tasks;

namespace OpenIDConnect.Users.Api.Controllers
{    
    [Route("api/[controller]")]
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

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserCreateApiModel userApiModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.HttpBadRequest();   // TODO: unprocessible entity response
            }

            await this.usersRepository.AddUser(userApiModel.ToDomainModel());
            return this.Ok();       // TODO: return created response
        }
        
        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            var user = await this.usersRepository.GetUser(userId);
            if (user == null)
            {
                return this.HttpNotFound();
            }

            var userApiModel = new UserApiModel { Id = user.Id };
            return this.Ok(userApiModel);
        }     

        [HttpPost("{userId}/authenticate")]
        public async Task<IActionResult> Authenticate(string userId, string password)
        {
            var passwordMatches = await this.usersRepository.Authenticate(userId, password);
            if (passwordMatches)
            {
                return this.Ok();
            }

            return this.HttpBadRequest();
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using OpenIDConnect.Clients.AngularMaterial.Models;
using Thinktecture.IdentityModel.Extensions;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization;

namespace OpenIDConnect.Clients.AngularMaterial
{
    public class AuthorizationManager : ResourceAuthorizationManager
    {
        public override Task<bool> CheckAccessAsync(ResourceAuthorizationContext context)
        {
            var resource = context.Resource.First().Value;

            switch (resource)
            {
                case ResourceTypes.Cartoons:
                    return CheckCartoonAccess(context);
                case ResourceTypes.Characters:
                    return CheckCharacterAccess(context);
                case ResourceTypes.Information:
                    return CheckInformationAccess(context);
            }

            return Nok();
        }

        public Task<bool> CheckCartoonAccess(ResourceAuthorizationContext context)
        {
            if (!context.Principal.Identity.IsAuthenticated)
            {
                return Nok();
            }

            return Ok();
        }

        private Task<bool> CheckCharacterAccess(ResourceAuthorizationContext context)
        {
            var action = context.Action.First().Value;

            if (context.Principal.HasClaim(action))
            {
                return Ok();
            }

            return Nok();
        }

        private Task<bool> CheckInformationAccess(ResourceAuthorizationContext context)
        {
            if (context.Principal.IsInRole("admin"))
            {
                return Ok();
            }

            return Nok();
        }
    }
}
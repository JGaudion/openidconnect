using AutoMapper;
using IdentityServer3.Core.Models;
using OpenIDConnect.Core.Models;
using System;
using System.Collections.Generic;
using static IdentityServer3.Core.Constants;
using DomainClient = OpenIDConnect.Core.Models.Client;
using DomainExternalIdentity = OpenIDConnect.Core.Models.ExternalIdentity;
using DomainSecret = OpenIDConnect.Core.Models.Secret;
using IdentityServerClient = IdentityServer3.Core.Models.Client;
using IdentityServerExternalIdentity = IdentityServer3.Core.Models.ExternalIdentity;
using IdentityServerSecret = IdentityServer3.Core.Models.Secret;

namespace OpenIDConnect.IdentityServer.Configuration
{
    public static class MappingConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<IdentityServerExternalIdentity, DomainExternalIdentity>()
                .ConstructUsing((IdentityServerExternalIdentity id) => 
                    new DomainExternalIdentity(id.Claims, id.Provider, id.ProviderId))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            Mapper.CreateMap<SignInMessage, SignInData>();

            Mapper.CreateMap<AuthenticationResult, AuthenticateResult>()
                .ConstructUsing((AuthenticationResult r) =>
                {
                    if (r.IsError)
                    {
                        return new AuthenticateResult(r.ErrorMessage);
                    }
                    if (r.IsPartialSignIn)
                    {
                        if (r.HasSubject)
                        {
                            return new AuthenticateResult(r.PartialSignInRedirectPath, r.Subject, r.Name, r.User.Claims, r.IdentityProvider, r.AuthenticationMethod);
                        }
                        else
                        {
                            return new AuthenticateResult(r.PartialSignInRedirectPath, r.User.Claims);
                        }
                    }
                    else if (r.HasSubject)
                    {
                        return new AuthenticateResult(r.Subject, r.Name, r.User.Claims, r.IdentityProvider, r.AuthenticationMethod);
                    }
                    else
                    {
                        throw new InvalidOperationException("Unable to map non-partial sign in with no subject or error");
                    }
                })
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            Mapper.CreateMap<AuthenticateResult, AuthenticationResult>()
                .ConstructUsing((AuthenticateResult r) =>
                {
                    if (r.IsError)
                    {
                        return new AuthenticationResult(r.ErrorMessage);
                    }
                    if (r.IsPartialSignIn)
                    {
                        if (r.HasSubject)
                        {
                            return new AuthenticationResult(r.PartialSignInRedirectPath,
                                r.User.FindFirst(ClaimTypes.Subject).Value,
                                r.User.FindFirst(ClaimTypes.Name).Value,
                                r.User.Claims,
                                r.User.FindFirst(ClaimTypes.IdentityProvider).Value,
                                r.User.FindFirst(ClaimTypes.AuthenticationMethod).Value);
                        }
                        else
                        {
                            return new AuthenticationResult(r.PartialSignInRedirectPath, r.User.Claims);
                        }
                    }
                    else if (r.HasSubject)
                    {
                        return new AuthenticationResult(r.User.FindFirst(ClaimTypes.Subject).Value,
                            r.User.FindFirst(ClaimTypes.Name).Value,
                            r.User.Claims, r.User.FindFirst(ClaimTypes.IdentityProvider).Value,
                            r.User.FindFirst(ClaimTypes.AuthenticationMethod).Value);
                    }
                    else
                    {
                        throw new InvalidOperationException("Unable to map non-partial sign in with no subject or error");
                    }
                })
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            Mapper.CreateMap<ProfileDataRequestContext, ProfileDataRequest>()
                .ConstructUsing((ProfileDataRequestContext c) =>
                {
                    if (c.AllClaimsRequested)
                    {
                        return new ProfileDataRequest(c.Subject, Mapper.Map<IdentityServerClient, DomainClient>(c.Client), c.Caller);
                    }
                    else
                    {
                        return new ProfileDataRequest(c.Subject, Mapper.Map<IdentityServerClient, DomainClient>(c.Client), c.Caller, c.RequestedClaimTypes);
                    }
                })
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            Mapper.CreateMap<IdentityServerClient, DomainClient>()
                .ConstructUsing((IdentityServerClient c) =>
                    new DomainClient(c.ClientName, c.ClientId, Mapper.Map<IEnumerable<IdentityServerSecret>, IEnumerable<DomainSecret>>(c.ClientSecrets), c.AllowedScopes))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            Mapper.CreateMap<IdentityServerSecret, DomainSecret>()
                .ConstructUsing((IdentityServerSecret s) =>
                    new DomainSecret(s.Value, s.Description, s.Type, s.Expiration));
        }
    }
}

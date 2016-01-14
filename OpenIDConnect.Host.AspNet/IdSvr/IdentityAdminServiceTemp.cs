using IdentityAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityAdmin.Core.Client;
using IdentityAdmin.Core.Metadata;
using IdentityAdmin.Core.Scope;

namespace OpenIDConnect.Host.AspNet.IdSvr
{
    public class IdentityAdminServiceTemp : IIdentityAdminService
    {
        public Task<IdentityAdminResult> AddClientClaimAsync(string subject, string type, string value)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult> AddClientCorsOriginAsync(string subject, string origin)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult> AddClientCustomGrantTypeAsync(string subject, string grantType)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult> AddClientIdPRestrictionAsync(string subject, string provider)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult> AddClientRedirectUriAsync(string subject, string uri)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult> AddClientScopeAsync(string subject, string scope)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult> AddClientSecretAsync(string subject, string type, string value)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult> AddPostLogoutRedirectUriAsync(string subject, string uri)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult> AddScopeClaimAsync(string subject, string name, string description, bool alwaysIncludeInIdToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult<CreateResult>> CreateClientAsync(IEnumerable<PropertyValue> properties)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult<CreateResult>> CreateScopeAsync(IEnumerable<PropertyValue> properties)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult> DeleteClientAsync(string subject)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult> DeleteScopeAsync(string subject)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult<ClientDetail>> GetClientAsync(string subject)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminMetadata> GetMetadataAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult<ScopeDetail>> GetScopeAsync(string subject)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult<QueryResult<ClientSummary>>> QueryClientsAsync(string filter, int start, int count)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult<QueryResult<ScopeSummary>>> QueryScopesAsync(string filter, int start, int count)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult> RemoveClientClaimAsync(string subject, string id)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult> RemoveClientCorsOriginAsync(string subject, string id)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult> RemoveClientCustomGrantTypeAsync(string subject, string id)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult> RemoveClientIdPRestrictionAsync(string subject, string id)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult> RemoveClientRedirectUriAsync(string subject, string id)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult> RemoveClientScopeAsync(string subject, string id)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult> RemoveClientSecretAsync(string subject, string id)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult> RemovePostLogoutRedirectUriAsync(string subject, string id)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult> RemoveScopeClaimAsync(string subject, string id)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult> SetClientPropertyAsync(string subject, string type, string value)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityAdminResult> SetScopePropertyAsync(string subject, string type, string value)
        {
            throw new NotImplementedException();
        }
    }
}

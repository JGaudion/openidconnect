namespace OpenIDConnect.Core.Constants
{
    public static class ClaimTypes
    {
        //IdentityServer
        public const string AccessTokenHash = "at_hash";
        public const string Address = "address";
        public const string Audience = "aud";
        public const string AuthenticationContextClassReference = "acr";
        public const string AuthenticationMethod = "amr";
        public const string AuthenticationTime = "auth_time";
        public const string AuthorizationCodeHash = "c_hash";
        public const string AuthorizationReturnUrl = "authorization_return_url";
        public const string AuthorizedParty = "azp";
        public const string BirthDate = "birthdate";
        public const string ClientId = "client_id";
        public const string Email = "email";
        public const string EmailVerified = "email_verified";
        public const string Expiration = "exp";
        public const string ExternalProviderUserId = "external_provider_user_id";
        public const string FamilyName = "family_name";
        public const string Gender = "gender";
        public const string GivenName = "given_name";
        public const string Id = "id";
        public const string IdentityProvider = "idp";
        public const string IssuedAt = "iat";
        public const string Issuer = "iss";
        public const string JwtId = "jti";
        public const string Locale = "locale";
        public const string MiddleName = "middle_name";
        public const string Name = "name";
        public const string NickName = "nickname";
        public const string Nonce = "nonce";
        public const string NotBefore = "nbf";
        public const string PartialLoginRestartUrl = "partial_login_restart_url";
        public const string PartialLoginResumeId = "partial_login_resume_id:{0}";
        public const string PartialLoginReturnUrl = "partial_login_return_url";
        public const string PhoneNumber = "phone_number";
        public const string PhoneNumberVerified = "phone_number_verified";
        public const string Picture = "picture";
        public const string PreferredUserName = "preferred_username";
        public const string Profile = "profile";
        public const string ReferenceTokenId = "reference_token_id";
        public const string Role = "role";
        public const string Scope = "scope";
        public const string Secret = "secret";
        public const string SessionId = "sid";
        public const string Subject = "sub";
        public const string UpdatedAt = "updated_at";
        public const string WebSite = "website";
        public const string ZoneInfo = "zoneinfo";

        //IdentityManager
        public const string BootstrapToken = "bootstrap-token";
        public const string Password = "password";
        public const string Phone = "phone";
        public const string Username = "username";

        public static readonly string[] OidcProtocolClaimTypes = new string[]
        {
            Subject,
            AuthenticationMethod,
            IdentityProvider,
            AuthenticationTime,
            Audience,
            Issuer,
            NotBefore,
            Expiration,
            UpdatedAt,
            IssuedAt,
            AuthenticationContextClassReference,
            AuthorizedParty,
            AccessTokenHash,
            AuthorizationCodeHash,
            Nonce,
            JwtId,
            Scope,
            SessionId
        };
    }
}

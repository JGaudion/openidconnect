namespace OpenIDConnect.Core.Constants
{
    /// <summary>
    /// Provides authentication types. Will need to be changed if not using idsrv
    /// </summary>
    public static class AuthenticationTypes
    {
        public const string PrimaryAuthenticationType = "idsrv";
        public const string ExternalAuthenticationType = "idsrv.external";
        public const string PartialSignInAuthenticationType = "idsrv.partial";
    }
}

namespace OpenIDConnect.Core.Api.Results
{
    using System.Net;

    using Microsoft.AspNet.Mvc;

    public class EntityCreatedResult : HttpStatusCodeResult
    {
        public EntityCreatedResult() : base((int)HttpStatusCode.Created)
        {
        }
    }
}
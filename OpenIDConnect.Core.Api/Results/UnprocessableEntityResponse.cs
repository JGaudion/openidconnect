namespace OpenIDConnect.Core.Api.Results
{
    using Microsoft.AspNet.Mvc;

    public class UnprocessableEntityResult : HttpStatusCodeResult
    {
        public UnprocessableEntityResult() : base(422)
        {
        }
    }
}
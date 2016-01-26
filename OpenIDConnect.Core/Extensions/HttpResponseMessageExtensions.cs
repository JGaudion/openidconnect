using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenIDConnect.Core.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static Task<TOut> DeserializeJsonContentAsync<TOut>(this HttpResponseMessage responseMessage)
        {
            if (responseMessage == null)
            {
                throw new ArgumentNullException(nameof(responseMessage));
            }

            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new ArgumentException("Response message must have success status code", nameof(responseMessage));
            }

            return Task.Run(async () => JsonConvert.DeserializeObject<TOut>(await responseMessage.Content.ReadAsStringAsync()));
        }
    }
}

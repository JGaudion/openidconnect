using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenIDConnect.Core.Models
{
    public class SignInData
    {
        public long Created { get; set; } = DateTimeOffset.UtcNow.Ticks;
        public IEnumerable<string> AcrValues { get; set; } = Enumerable.Empty<string>();
        public string ClientId { get; set; }
        public string DisplayMode { get; set; }
        public string IdP { get; set; }
        public string LoginHint { get; set; }
        public string ReturnUrl { get; set; }
        public string Tenant { get; set; }
        public string UiLocales { get; set; }
    }
}

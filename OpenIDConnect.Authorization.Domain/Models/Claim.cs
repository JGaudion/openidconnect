namespace OpenIDConnect.Authorization.Domain.Models
{
    public class Claim
    {
        public Claim(string type, string value)
        {
            this.Type = type;
            this.Value = value;
        }

        public string Type { get; }

        public string Value { get; }
    }
}

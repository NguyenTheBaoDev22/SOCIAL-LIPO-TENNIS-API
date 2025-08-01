using System.Text.RegularExpressions;

namespace API.Conventions
{
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string? TransformOutbound(object? value)
        {
            if (value == null) return null;

            // Convert "ClientCredential" => "client-credential"
            return Regex.Replace(value.ToString()!, "([a-z])([A-Z])", "$1-$2").ToLowerInvariant();
        }
    }
}

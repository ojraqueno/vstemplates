using System;

namespace Microsoft.Extensions.Configuration
{
    public static class IConfigurationExtensions
    {
        public static bool? GetNullableBool(this IConfiguration configuration, string key)
        {
            if (String.IsNullOrWhiteSpace(key)) throw new ArgumentException(nameof(key));

            return bool.TryParse(configuration[key], out bool result) ? result : (bool?)null;
        }
    }
}
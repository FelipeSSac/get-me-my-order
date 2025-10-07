using System.Text.RegularExpressions;

namespace Worker.Extensions;

public static class ConfigurationHelper
{
    public static string ExpandEnvironmentVariables(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return Regex.Replace(input, @"\$\{([^}]+)\}", match =>
        {
            var envVarName = match.Groups[1].Value;
            return Environment.GetEnvironmentVariable(envVarName) ?? match.Value;
        });
    }
}
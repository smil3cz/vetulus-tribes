using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

namespace GreenFoxAcademy.SpaceSettlers.Helpers
{
    public static class AppSettings
    {
        public static string? ConnectionString
        {
            get => Environment.GetEnvironmentVariable("ConnectionString");
        }
        public static string? JwtSecret
        {
            get => Environment.GetEnvironmentVariable("JwtKey");
        }
        public static string? JwtIssuer
        {
            get => Environment.GetEnvironmentVariable("Issuer");
        }
        public static string? EnvironmentVariable
        {
            get => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }
        public static string? LogFilePath
        {
            get => Environment.GetEnvironmentVariable("LogFilePath");
        }

        public static string? SendGridAPIKey
        {
            get => Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
        }

        public static void GetDataFromSettings()
        {
            var file = File.OpenText("Properties/launchSettings.json");
            var reader = new JsonTextReader(file);
            var jObject = JObject.Load(reader);

            var variables = jObject.GetValue("profiles").SelectMany(profiles => profiles.Children())
                .SelectMany(profile => profile.Children<JProperty>()).Where(prop => prop.Name == "environmentVariables")
                .SelectMany(prop => prop.Value.Children<JProperty>()).ToList();

            foreach (var variable in variables)
            {
                Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
            }
        }
    }
}

using Auth0.ManagementApi.Models.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Models;
public class ConfigureServerArgument
{
    public string ServerDomain { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public List<ConfigureServerActionArgument> ConfigureServerActionArguments { get; set; }

    public ConfigureServerArgument(List<ConfigureServerActionArgument> configureServerActionArguments, string clientSecret, string clientId, string serverDomain)
    {
        ConfigureServerActionArguments = configureServerActionArguments;
        ClientSecret = clientSecret;
        ClientId = clientId;
        ServerDomain = serverDomain;
    }

    public class ConfigureServerActionArgument
    {
        public string Name { get; set; }
        public string FileLocation { get; set; }
        public List<ActionSecret> Secrets { get; set; }

        public ConfigureServerActionArgument(List<ActionSecret> secrets, string fileLocation, string name)
        {
            Secrets = secrets;
            FileLocation = fileLocation;
            Name = name;
        }
    }
}

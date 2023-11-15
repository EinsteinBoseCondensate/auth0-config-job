using Auth0.ManagementApi.Models.Actions;
using ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Extensions;
internal static class CliExtensions
{
    private const string CustomFieldsSignUpProcessActionName = "CustomFieldsSignUpProcess";
    internal static ConfigureServerArgument ToConfigureServerArgument(this string[] cliArguments)
    {
        if (cliArguments.Length != 6)
            throw new Exception("Invalid cli arguments");

        var serverDomain = cliArguments[1];
        var clientId = cliArguments[2];
        var clientSecret = cliArguments[3];
        var customFieldsSignUpProcessActionSecretSigningKeyForJwtValue = cliArguments[4];
        var customFieldsSignUpProcessActionSecretCustomFieldsSiteUrlValue = cliArguments[5];

        return new ConfigureServerArgument(
        BuildActionArguments(
            customFieldsSignUpProcessActionSecretSigningKeyForJwtValue,
            customFieldsSignUpProcessActionSecretCustomFieldsSiteUrlValue
            ),
        clientSecret,
        clientId,
        serverDomain
        );
    }
    private static List<ConfigureServerArgument.ConfigureServerActionArgument> BuildActionArguments(
        string customFieldsSignUpProcessActionSecretSigningKeyForJwtValue,
        string customFieldsSignUpProcessActionSecretCustomFieldsSiteUrlValue)
        => new List<ConfigureServerArgument.ConfigureServerActionArgument>
        {
            new ConfigureServerArgument.ConfigureServerActionArgument(
                    new List<ActionSecret>
                    {
                        new ActionSecret()
                        {
                            Name = "JwtSigningKey",
                            Value = customFieldsSignUpProcessActionSecretSigningKeyForJwtValue

                        },
                        new ActionSecret()
                        {
                            Name = "CustomFieldsSiteUrl",
                            Value = customFieldsSignUpProcessActionSecretCustomFieldsSiteUrlValue

                        }
                    },
                    "./Resources/Actions/CustomFieldsSignUpProcess.js",
                    CustomFieldsSignUpProcessActionName
                    )
        };
}

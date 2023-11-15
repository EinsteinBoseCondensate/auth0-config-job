using System;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models.Actions;
using ConsoleApp.Models;

namespace Auth0UniversalTemplateDeployer.Services
{
    public static class Auth0Service
    {
        
        private static async Task<string> GetAccessTokenForManagementApi(string domain, string clientId, string clientSecret)
        {
            try
            {
                var authenticationApiClient = new AuthenticationApiClient(new Uri($"https://{domain}"));

                var tokenRequest = new ClientCredentialsTokenRequest
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    Audience = $"https://{domain}/api/v2/"
                };
                var response = await authenticationApiClient.GetTokenAsync(tokenRequest).ConfigureAwait(false);
                return response.AccessToken;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error fetching token: {e.Message ?? e.InnerException?.Message}");
                return string.Empty;
            }

        }

        public static async Task ConfigureServer(ConfigureServerArgument configureServerArgument)
        {
            try
            {
                var accessToken = await GetAccessTokenForManagementApi(configureServerArgument.ServerDomain, configureServerArgument.ClientId, configureServerArgument.ClientSecret)
                    .ConfigureAwait(false);
                var managementApiClient = new ManagementApiClient(accessToken, configureServerArgument.ServerDomain);
                await Task.WhenAll(configureServerArgument.ConfigureServerActionArguments.Select(argument => managementApiClient.UpdateAndDeployAction(argument)));
                
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error configuring auth0 server: ${e.Message ?? e.InnerException?.Message}");
            }
        }

        private static async Task UpdateAndDeployAction(this ManagementApiClient managementApiClient, ConfigureServerArgument.ConfigureServerActionArgument configureServerActionArgument)
        {
            try
            {
                var customFieldsSignUpProcessAction = await managementApiClient.Actions.GetAllAsync(new GetActionsRequest
                {
                    ActionName = configureServerActionArgument.Name
                }, new Auth0.ManagementApi.Paging.PaginationInfo { });
                var actionId = customFieldsSignUpProcessAction.FirstOrDefault()?.Id;
                if (string.IsNullOrEmpty(actionId))
                    throw new Exception($"Action {configureServerActionArgument.Name} couldn't be retrieved");
                await managementApiClient.Actions.UpdateAsync(actionId, new UpdateActionRequest
                {
                    Code = await File.ReadAllTextAsync(configureServerActionArgument.FileLocation),
                    Secrets = configureServerActionArgument.Secrets
                });
                await managementApiClient.Actions.DeployAsync(actionId);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error configuring auth0 action: ${e.Message ?? e.InnerException?.Message}");
            }
        }
    }
}

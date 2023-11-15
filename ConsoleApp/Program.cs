using Auth0UniversalTemplateDeployer.Services;
using ConsoleApp.Extensions;

await Auth0Service.ConfigureServer(Environment.GetCommandLineArgs().ToConfigureServerArgument()).ConfigureAwait(false);
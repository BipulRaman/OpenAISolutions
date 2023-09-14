using AOAI.Solution.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureAppConfiguration((hostContext, config) =>
    {
        _ = config.AddJsonFile("appsettings.json", optional: true);
        _ = config.AddJsonFile("appsettings.local.json", optional: true);
        _ = config.AddEnvironmentVariables();
    })
    .ConfigureServices((hostContext, services) =>
    {

        _ = services.AddAOAISolutionHelper(hostContext.Configuration);
    })
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();

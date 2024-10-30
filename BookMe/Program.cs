using BookMe;
using BookMe.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = CreateHostBuilder(args).Build();
using var scope = host.Services.CreateScope();

var services = scope.ServiceProvider;

try
{
    services.GetRequiredService<App>().Run(args);
}
catch (Exception e)
{
    Console.WriteLine($"An error occurred: {e.Message}");
}

return;

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            services.AddSingleton<App>();
            services.AddSingleton<ArgsResolver>();
            services.AddSingleton<IFileReader, JsonFileReader>();
            services.AddSingleton<AvailabilityService>();
            services.AddSingleton<DateParser>();
        });

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotificationSystem.Services.Interfaces;
using NotificationSystem.Services.Repositories;

var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    // Register your services here
                    services.AddTransient<INotification, NotificationRepository>();
                    services.AddTransient<IMainService, MainService>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .Build();

// Run the service
Thread thread = new(() =>
{
    var main = ActivatorUtilities.CreateInstance<MainService>(host.Services);
    main?.Run();
});

thread.IsBackground = true;
thread.Start();

host.Run();
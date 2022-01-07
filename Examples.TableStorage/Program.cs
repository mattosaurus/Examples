using Azure.Identity;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System;
using System.Globalization;

namespace Examples.TableStorage
{
    class Program
    {
        public static IConfigurationRoot? configuration;

        static int Main(string[] args)
        {
            // Build configuration
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            // SQL sink configuration
            ColumnOptions columnOptions = new ColumnOptions();
            // Don't include the Properties XML column.
            columnOptions.Store.Remove(StandardColumn.Properties);
            // Do include the log event data as JSON.
            columnOptions.Store.Add(StandardColumn.LogEvent);

            // Initialize serilog logger
            Log.Logger = new LoggerConfiguration()
                 .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
                 .MinimumLevel.Debug()
                 .Enrich.FromLogContext()
                 .CreateLogger();

            try
            {
                // Start!
                MainAsync(args).Wait();
                return 0;
            }
            catch
            {
                return 1;
            }
        }

        static async Task MainAsync(string[] args)
        {
            // Create service collection
            Log.Information("Creating service collection");
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Create service provider
            Log.Information("Building service provider");
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            try
            {
                Log.Information("Starting service");
                await serviceProvider.GetService<App>().RunAsync();
                Log.Information("Ending service");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Error running service");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Add logging
            serviceCollection.AddSingleton(LoggerFactory.Create(builder =>
            {
                builder
                    .AddSerilog(dispose: true);
            }));

            serviceCollection.AddLogging();

            if (configuration == null)
            {
                throw new ArgumentNullException("No configuration found");
            }

            // Add access to generic IConfigurationRoot
            serviceCollection.AddSingleton<IConfigurationRoot>(configuration);

            // Set AZURE_TENANT_ID enviroment variable to the tenant ID that contains the resource
            // Only needed for when debugging locally
            Environment.SetEnvironmentVariable("AZURE_TENANT_ID", configuration["AZURE_TENANT_ID"]);

            // Add table storage client
            serviceCollection.AddAzureClients(builder =>
            {
                // Add a storage account client
                builder.AddTableServiceClient(new Uri(configuration["TableStorageUri"]));

                // Select the appropriate credentials based on enviroment
                builder.UseCredential(new DefaultAzureCredential());
            });

            // Add app
            serviceCollection.AddTransient<App>();
        }
    }
}
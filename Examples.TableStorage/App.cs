using Azure.Data.Tables;
using Examples.TableStorage.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.TableStorage
{
    public class App
    {
        private readonly TableServiceClient _tableServiceClient;
        private readonly ILogger<App> _logger;

        public App(TableServiceClient tableServiceClient, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<App>();
            _tableServiceClient = tableServiceClient;
        }

        public async Task RunAsync()
        {
            TableClient tableClient = _tableServiceClient.GetTableClient("Stores");
            var store = await tableClient.GetEntityAsync<StoreEntity>("0006", "1");
        }
    }
}

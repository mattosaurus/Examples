using Examples.SqlConnection.Models;
using Examples.SqlConnection.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.SqlConnection
{
    public class App
    {
        private readonly IStoreService _storeClient;
        private readonly ILogger<App> _logger;

        public App(IStoreService storeClient, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<App>();
            _storeClient = storeClient;
        }

        public async Task RunAsync()
        {
            List<Store> stores = await _storeClient.GetStoresAsync();
        }
    }
}

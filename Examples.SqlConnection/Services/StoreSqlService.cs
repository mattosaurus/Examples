using Examples.SqlConnection.Helpers;
using Examples.SqlConnection.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.SqlConnection.Services
{
    public class StoreSqlService : IStoreService
    {
        private readonly ISqlConnectionFactory _dbConnectionFactory;
        private readonly ILogger<StoreSqlService> _logger;

        public StoreSqlService(ISqlConnectionFactory dbConnectionFactory, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<StoreSqlService>();
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<List<Store>> GetStoresAsync()
        {
            List<Store> stores = new List<Store>();

            using (System.Data.SqlClient.SqlConnection connection = await GetSqlConnectionAsync())
            {
                using (SqlCommand command = new SqlCommand("sp__get_stores"))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandTimeout = 6000;
                    command.Connection = connection;

                    await connection.OpenAsync();
                    
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                stores.Add(new Store()
                                {
                                    Name = reader["Name"].ToString(),
                                    Id = reader["Id"].ToString(),
                                    Region = reader["Region"].ToString()
                                });
                            }
                        }
                    }

                    await connection.CloseAsync();
                }
            }

            return stores;
        }

        private System.Data.SqlClient.SqlConnection GetSqlConnection()
        {
            return (System.Data.SqlClient.SqlConnection)_dbConnectionFactory.CreateSqlConnection("DataConnection", true);
        }

        private async Task<System.Data.SqlClient.SqlConnection> GetSqlConnectionAsync()
        {
            return (System.Data.SqlClient.SqlConnection)await _dbConnectionFactory.CreateSqlConnectionAsync("DataConnection", true);
        }

        private System.Data.SqlClient.SqlConnection GetSqlConnection(string connectionName)
        {
            return (System.Data.SqlClient.SqlConnection)_dbConnectionFactory.CreateSqlConnection(connectionName, true);
        }

        private async Task<System.Data.SqlClient.SqlConnection> GetSqlConnectionAsync(string connectionName)
        {
            return (System.Data.SqlClient.SqlConnection)await _dbConnectionFactory.CreateSqlConnectionAsync(connectionName, true);
        }
    }
}

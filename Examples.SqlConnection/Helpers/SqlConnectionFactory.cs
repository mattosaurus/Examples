using Azure.Core;
using Azure.Identity;
using Examples.SqlConnection.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.SqlConnection.Helpers
{
    public interface ISqlConnectionFactory
    {
        IDbConnection CreateSqlConnection(string connectionName, bool useAzureIdentity = false);

        Task<IDbConnection> CreateSqlConnectionAsync(string connectionName, bool useAzureIdentity = false);
    }

    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly DefaultAzureCredential _credential;
        private readonly IDictionary<string, string> _connectionDictionary;
        private readonly TokenRequestContext _tokenRequestContext;
        private readonly SqlConnectionOptions _options;

        public SqlConnectionFactory(IOptions<SqlConnectionOptions> options)
        {
            _options = options.Value;
            _connectionDictionary = _options.Connections;
            _tokenRequestContext = new TokenRequestContext(_options.Scopes, tenantId: _options.AzureTenantId);
            _credential = new DefaultAzureCredential();
        }

        public IDbConnection CreateSqlConnection(string connectionName, bool useAzureIdentity = false)
        {
            return CreateSqlConnectionAsync(connectionName, useAzureIdentity).Result;
        }

        public async Task<IDbConnection> CreateSqlConnectionAsync(string connectionName, bool useAzureIdentity = false)
        {
            if (useAzureIdentity)
            {
                AccessToken accessToken = await _credential.GetTokenAsync(_tokenRequestContext);
                return new System.Data.SqlClient.SqlConnection(GetConnectionString(connectionName))
                {
                    AccessToken = accessToken.Token
                };
            }
            else
            {
                return new System.Data.SqlClient.SqlConnection(GetConnectionString(connectionName));
            }
        }

        private string GetConnectionString(string connectionName)
        {
            _connectionDictionary.TryGetValue(connectionName, out string connectionString);

            if (connectionString == null)
            {
                throw new Exception(string.Format("Connection string {0} was not found", connectionName));
            }

            return connectionString;
        }
    }
}

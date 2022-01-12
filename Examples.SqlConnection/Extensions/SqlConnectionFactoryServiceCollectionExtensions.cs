using Examples.SqlConnection.Helpers;
using Examples.SqlConnection.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.SqlConnection.Extensions
{
    public static class SqlConnectionFactoryServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlConnectionFactory(this IServiceCollection collection, Action<SqlConnectionOptions> setupAction)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));

            collection.Configure(setupAction);
            return collection.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        }

        public static IServiceCollection AddSqlConnectionFactory(this IServiceCollection collection, Dictionary<string, string> connections, string[] scopes = null, string tenantId = null)
        {
            return AddSqlConnectionFactory(collection, builder => {
                builder.Connections = connections;

                if (scopes != null)
                    builder.Scopes = scopes;

                if (tenantId != null)
                    builder.AzureTenantId = tenantId;
            });
        }
    }
}

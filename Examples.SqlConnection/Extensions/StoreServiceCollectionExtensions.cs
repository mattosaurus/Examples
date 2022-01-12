using Examples.SqlConnection.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.SqlConnection.Extensions
{
    public static class StoreServiceCollectionExtensions
    {
        public static IServiceCollection AddSql(this IServiceCollection collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            return collection.AddSingleton<IStoreService, StoreSqlService>();
        }
    }
}

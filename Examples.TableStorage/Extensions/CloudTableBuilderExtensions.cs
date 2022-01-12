using Azure;
using Azure.Core.Extensions;
using Azure.Data.Tables;
using Examples.TableStorage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.TableStorage.Extensions
{
	/// <summary>
	/// Extension methods to add <see cref="TableServiceClient"/> and <see cref="TableClient"/> instances to clients builder
	/// </summary>
	public static class TableClientBuilderExtensions
	{
		/// <summary>
		/// Registers a <see cref="TableServiceClient"/> instance with the provided <paramref name="connectionString"/>
		/// </summary>
		/// <param name="connectionString">Master connection string for accessing the storage account</param>
		/// <param name="setupAction">Configuration options</param>
		/// <returns></returns>
		public static IAzureClientBuilder<TableServiceClient, StoreOptions> AddTableServiceClient<TBuilder>(this TBuilder builder, string connectionString, Action<StoreOptions> setupAction)
			where TBuilder : IAzureClientFactoryBuilder
		{
			if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));

			return builder.RegisterClientFactory<TableServiceClient, StoreOptions>(options => new TableServiceClient(connectionString));
		}

		/// <summary>
		/// Registers a <see cref="TableServiceClient"/> instance with the provided <paramref name="connectionString"/>
		/// </summary>
		/// <param name="connectionString">Master connection string for accessing the storage account</param>
		/// <returns></returns>
		public static IAzureClientBuilder<TableServiceClient, StoreOptions> AddTableServiceClient<TBuilder>(this TBuilder builder, string connectionString)
			where TBuilder : IAzureClientFactoryBuilder
		{
			return AddTableServiceClient(builder, connectionString, options => { });
		}

		/// <summary>
		/// Registers a <see cref="TableClient"/> instance
		/// </summary>
		/// <param name="storageUri">Base URI of the table storage container, e.g. https://mycontainer.table.core.windows.net</param>
		/// <param name="sasToken">SAS token scoped to the table to be accessed</param>
		/// <param name="tableName">Name of the table to be accessed</param>
		/// <param name="setupAction">Configuration options</param>
		/// <returns></returns>
		public static IAzureClientBuilder<TableClient, StoreOptions> AddTableClient<TBuilder>(this TBuilder builder, Uri storageUri, string sasToken, string tableName, Action<StoreOptions> setupAction)
			where TBuilder : IAzureClientFactoryBuilder
		{
			if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));

			return builder.RegisterClientFactory<TableClient, StoreOptions>(options => new TableServiceClient(storageUri, new AzureSasCredential(sasToken)).GetTableClient(tableName));
		}

		/// <summary>
		/// Registers a <see cref="TableClient"/> instance
		/// </summary>
		/// <param name="storageUri">Base URI of the table storage container, e.g. https://mycontainer.table.core.windows.net</param>
		/// <param name="sasToken">SAS token scoped to the table to be accessed</param>
		/// <param name="tableName">Name of the table to be accessed</param>
		/// <returns></returns>
		public static IAzureClientBuilder<TableClient, StoreOptions> AddTableClient<TBuilder>(this TBuilder builder, Uri storageUri, string sasToken, string tableName)
			where TBuilder : IAzureClientFactoryBuilder
		{
			return AddTableClient(builder, storageUri, sasToken, tableName, options => { });
		}
	}
}

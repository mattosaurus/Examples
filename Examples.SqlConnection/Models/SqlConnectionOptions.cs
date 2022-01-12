using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.SqlConnection.Models
{
    public class SqlConnectionOptions
    {
        public Dictionary<string, string> Connections { get; set; }

        public string AzureTenantId { get; set; }

        public string[] Scopes { get; set; } = new[] { "https://database.windows.net/.default" };
    }
}

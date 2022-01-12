using Examples.SqlConnection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.SqlConnection.Services
{
    public interface IStoreService
    {
        Task<List<Store>> GetStoresAsync();
    }
}

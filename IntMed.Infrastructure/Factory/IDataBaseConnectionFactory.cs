using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntMed.Infrastructure.Factory
{
    public interface IDataBaseConnectionFactory
    {
        Task<NpgsqlConnection> GetConnectionFactoryAsync();
    }
}

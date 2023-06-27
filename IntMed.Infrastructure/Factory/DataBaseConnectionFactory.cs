using IntMed.Infrastructure.Factory;
using Npgsql;

namespace IntMed.Infrasctructure.Factory
{
    public class DataBaseConnectionFactory :IDataBaseConnectionFactory
    {
        private string ConnectionString { get; }

        public DataBaseConnectionFactory(string connectionString) =>
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

        public async Task<NpgsqlConnection> GetConnectionFactoryAsync()
        {
            var connection = new NpgsqlConnection(ConnectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}

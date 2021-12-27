using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using DotNetCore.CAP.Persistence;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Npgsql;

namespace DotNetCore.CAP.HealthCheck.PostgreSql.Connection
{
    public class PostgreSqlConnectionHealthCheck : IHealthCheck
    {
        private readonly IStorageInitializer _initializer;
        private readonly IDbConnection _connection;

        public PostgreSqlConnectionHealthCheck(
            IOptions<PostgreSqlOptions> options,
            IStorageInitializer initializer)
        {
            _initializer = initializer;
            _connection = new NpgsqlConnection(options.Value.ConnectionString);
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            HealthCheckResult result;

            try
            {
                _connection.Open();

                string publishedQuery = $@"SELECT COUNT(*) FROM {_initializer.GetPublishedTableName()} LIMIT 1";
                string receivedQuery = $@"SELECT COUNT(*) FROM {_initializer.GetReceivedTableName()} LIMIT 1";
                
                await _connection.QueryAsync(publishedQuery);
                await _connection.QueryAsync(receivedQuery);

                result = HealthCheckResult.Healthy();
            }
            catch
            {
                result = HealthCheckResult.Unhealthy();
            }
            finally
            {
                if (_connection is not null && _connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                    _connection.Dispose();
                }
            }

            return result;
        }
    }
}
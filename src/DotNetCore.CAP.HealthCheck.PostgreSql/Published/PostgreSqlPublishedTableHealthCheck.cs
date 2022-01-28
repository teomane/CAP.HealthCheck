using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using DotNetCore.CAP.Persistence;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Npgsql;

namespace DotNetCore.CAP.HealthCheck.PostgreSql.Published
{
    public class PostgreSqlPublishedTableHealthCheck : IHealthCheck
    {
        private readonly IStorageInitializer _initializer;
        private readonly IDbConnection _connection;

        public PostgreSqlPublishedTableHealthCheck(
            IOptions<PostgreSqlOptions> options,
            IStorageInitializer initializer)
        {
            _initializer = initializer;
            _connection = new NpgsqlConnection(options.Value.ConnectionString);
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            string query =
                $@"SELECT * FROM {_initializer.GetPublishedTableName()} where ""StatusName"" != 'Succeeded' order by ""Added"" desc limit 100;";
            
            HealthCheckResult result;

            try
            {
                _connection.Open();

                List<PostgreSqlPublishedTableData> data =
                    (await _connection.QueryAsync<PostgreSqlPublishedTableData>(query)).ToList();

                result = data.Any()
                    ? HealthCheckResult.Unhealthy($"Failed Count: {data.Count}",
                        data: new Dictionary<string, object>()
                        {
                            { "Published", JsonSerializer.Serialize(data) }
                        })
                    : HealthCheckResult.Healthy();
            }
            catch (Exception e)
            {
                result = HealthCheckResult.Unhealthy(e.Message, e);
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
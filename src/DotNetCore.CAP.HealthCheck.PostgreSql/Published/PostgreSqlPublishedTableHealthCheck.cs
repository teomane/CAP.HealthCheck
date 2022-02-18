using System;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP.Monitoring;
using DotNetCore.CAP.Persistence;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DotNetCore.CAP.HealthCheck.PostgreSql.Published
{
    public class PostgreSqlPublishedTableHealthCheck : IHealthCheck
    {
        private readonly IMonitoringApi _monitoringApi;

        public PostgreSqlPublishedTableHealthCheck(IDataStorage dataStorage)
        {
            _monitoringApi = dataStorage.GetMonitoringApi();
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            HealthCheckResult result;

            int failedCount = _monitoringApi.PublishedFailedCount();

            try
            {
                result = failedCount > 0
                    ? HealthCheckResult.Unhealthy($"Failed Count: {failedCount}")
                    : HealthCheckResult.Healthy();
            }
            catch (Exception e)
            {
                result = HealthCheckResult.Unhealthy(e.Message, e);
            }

            return Task.FromResult(result);
        }
    }
}
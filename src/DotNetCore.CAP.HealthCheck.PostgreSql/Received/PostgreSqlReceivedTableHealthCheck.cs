using System;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP.Monitoring;
using DotNetCore.CAP.Persistence;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DotNetCore.CAP.HealthCheck.PostgreSql.Received;

public class PostgreSqlReceivedTableHealthCheck : IHealthCheck
{
    private readonly IMonitoringApi _monitoringApi;

    public PostgreSqlReceivedTableHealthCheck(
        IDataStorage dataStorage)
    {
        _monitoringApi = dataStorage.GetMonitoringApi();
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new CancellationToken())
    {
        HealthCheckResult result;

        try
        {
            int failedCount = _monitoringApi.ReceivedFailedCount();

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
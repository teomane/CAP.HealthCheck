using System;
using System.Collections.Generic;
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

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new CancellationToken())
    {
        HealthCheckResult result;

        try
        {
            int failedCount = await _monitoringApi.ReceivedFailedCount();
            int succeededCount = await _monitoringApi.ReceivedSucceededCount();

            var data = new Dictionary<string, object>()
            {
                {"failedCount", failedCount},
                {"succeededCount", succeededCount}
            };

            result = failedCount > 0
                ? HealthCheckResult.Unhealthy($"Failed Count: {failedCount}", data: data)
                : HealthCheckResult.Healthy(data: data);
        }
        catch (Exception e)
        {
            result = HealthCheckResult.Unhealthy(e.Message, e);
        }

        return result;
    }
}
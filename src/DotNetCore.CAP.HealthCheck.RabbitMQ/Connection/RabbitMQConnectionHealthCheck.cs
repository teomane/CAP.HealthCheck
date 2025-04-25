using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace DotNetCore.CAP.HealthCheck.RabbitMQ.Connection;

public class RabbitMQConnectionHealthCheck : IHealthCheck
{
    private readonly RabbitMQOptions _options;

    public RabbitMQConnectionHealthCheck(IOptions<RabbitMQOptions> options)
    {
        _options = options.Value;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            var connection = await CreateConnectionFactory(_options, cancellationToken);
            await using (connection)
            {
                var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);
                await using (channel)
                {
                    return HealthCheckResult.Healthy();
                }
            }
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
        }
    }

    private Task<IConnection> CreateConnectionFactory(RabbitMQOptions rabbitMqOptions, CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory()
        {
            UserName = rabbitMqOptions.UserName,
            Password = rabbitMqOptions.Password,
            HostName = rabbitMqOptions.HostName,
            Port = rabbitMqOptions.Port,
            VirtualHost = rabbitMqOptions.VirtualHost
        };

        return factory.CreateConnectionAsync(cancellationToken);
    }
}

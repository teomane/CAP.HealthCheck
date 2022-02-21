using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace DotNetCore.CAP.HealthCheck.RabbitMQ.Connection;

public class RabbitMQConnectionHealthCheck : IHealthCheck
{
    private readonly IConnection _connection;

    public RabbitMQConnectionHealthCheck(IOptions<RabbitMQOptions> options)
    {
        _connection = CreateConnectionFactory(options.Value);
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            using (_connection.CreateModel())
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy());
            }
        }
        catch (Exception ex)
        {
            return Task.FromResult(
                new HealthCheckResult(context.Registration.FailureStatus, exception: ex));
        }
    }

    private IConnection CreateConnectionFactory(RabbitMQOptions rabbitMqOptions)
    {
        var factory = new ConnectionFactory()
        {
            UserName = rabbitMqOptions.UserName,
            Password = rabbitMqOptions.Password,
            HostName = rabbitMqOptions.HostName,
            Port = rabbitMqOptions.Port,
            VirtualHost = rabbitMqOptions.VirtualHost
        };

        return factory.CreateConnection();
    }
}
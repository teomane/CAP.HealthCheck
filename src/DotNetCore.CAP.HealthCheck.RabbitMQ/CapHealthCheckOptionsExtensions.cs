using System;
using System.Collections.Generic;
using DotNetCore.CAP.HealthCheck.RabbitMQ.Connection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DotNetCore.CAP.HealthCheck
{
    public static class CapHealthCheckOptionsExtensions
    {
        public static CapHealthCheckOptions AddRabbitMQConnectionCheck(
            this CapHealthCheckOptions options,
            string name = "cap.rabbitmq.connection",
            HealthStatus? failureStatus = null,
            IEnumerable<string> tags = null,
            TimeSpan? timeout = null)
        {
            tags ??= new List<string> {"cap"};
            
            options.RegisterExtension(new RabbitMQConnectionHealthCheckOptionsExtension(name, failureStatus, tags, timeout));

            return options;
        }
    }
}
using System;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetCore.CAP.HealthCheck;

public static class ConfigureServiceExtensions
{
    public static void AddCapHealthCheck(this IHealthChecksBuilder healthChecksBuilder,
        Action<CapHealthCheckOptions> setupAction)
    {
        var options = new CapHealthCheckOptions();
        setupAction(options);

        foreach (var serviceExtension in options.Extensions)
        {
            serviceExtension.AddCheck(healthChecksBuilder);
        }

        healthChecksBuilder.Services.Configure(setupAction);
    }
}
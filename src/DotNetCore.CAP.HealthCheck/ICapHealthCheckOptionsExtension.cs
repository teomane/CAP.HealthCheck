using Microsoft.Extensions.DependencyInjection;

namespace DotNetCore.CAP.HealthCheck;

public interface ICapHealthCheckOptionsExtension
{
    void AddCheck(IHealthChecksBuilder builder);
}
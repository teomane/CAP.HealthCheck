using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace DotNetCore.CAP.HealthCheck
{
    public static class ConfigureExtensions
    {
        public static void MapCapHealthChecks(this IEndpointRouteBuilder endpoints, string pattern = "/health-cap")
        {
            endpoints.MapHealthChecks(pattern, new HealthCheckOptions()
            {
                Predicate = (check) => check.Tags.Contains("cap"),
                ResponseWriter = ResponseUtil.WriteResponse
            });
        }
    }
}
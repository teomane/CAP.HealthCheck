using System;
using System.Collections.Generic;
using DotNetCore.CAP.HealthCheck.MongoDB.Connection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DotNetCore.CAP.HealthCheck.MongoDB
{
    public static class CapHealthCheckOptionsExtensions
    {
        public static CapHealthCheckOptions AddMongoDBConnectionCheck(
            this CapHealthCheckOptions options,
            string name = "cap.mongodb.connection",
            HealthStatus? failureStatus = null,
            IEnumerable<string> tags = null,
            TimeSpan? timeout = null)
        {
            tags ??= new List<string> {"cap"};
            
            options.RegisterExtension(new MongoDBConnectionHealthCheckOptionsExtension(name, failureStatus, tags, timeout));

            return options;
        }
    }
}
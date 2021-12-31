using System;
using System.Collections.Generic;
using DotNetCore.CAP.HealthCheck.PostgreSql.Connection;
using DotNetCore.CAP.HealthCheck.PostgreSql.Published;
using DotNetCore.CAP.HealthCheck.PostgreSql.Received;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DotNetCore.CAP.HealthCheck
{
    public static class CapHealthCheckOptionsExtensions
    {
        public static CapHealthCheckOptions AddPostgreSqlConnectionCheck(
            this CapHealthCheckOptions options,
            string name = "cap.postgres.connection",
            HealthStatus? failureStatus = null,
            IEnumerable<string> tags = null,
            TimeSpan? timeout = null)
        {
            tags ??= new List<string> {"cap"};
            
            options.RegisterExtension(new PostgreSqlConnectionCapHealthCheckOptionsExtension(name, failureStatus, tags, timeout));

            return options;
        }

        public static CapHealthCheckOptions AddPostgreSqlPublishedTableCheck(
            this CapHealthCheckOptions options,
            string name = "cap.postgres.publishedtable",
            HealthStatus? failureStatus = null,
            IEnumerable<string> tags = null,
            TimeSpan? timeout = null)
        {
            tags ??= new List<string> {"cap"};
            
            options.RegisterExtension(
                new PostgreSqlPublishedTableHealthCheckOptionsExtension(name, failureStatus, tags, timeout));

            return options;
        }

        public static CapHealthCheckOptions AddPostgreSqlReceivedTableCheck(
            this CapHealthCheckOptions options,
            string name = "cap.postgres.receivedtable",
            HealthStatus? failureStatus = null,
            IEnumerable<string> tags = null,
            TimeSpan? timeout = null)
        {
            tags ??= new List<string> {"cap"};
            
            options.RegisterExtension(
                new PostgreSqlReceivedTableHealthCheckOptionsExtension(name, failureStatus, tags, timeout));

            return options;
        }
    }
}
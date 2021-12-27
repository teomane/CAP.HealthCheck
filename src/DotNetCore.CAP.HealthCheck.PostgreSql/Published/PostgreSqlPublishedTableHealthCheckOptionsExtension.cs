using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DotNetCore.CAP.HealthCheck.PostgreSql.Published
{
    public class PostgreSqlPublishedTableHealthCheckOptionsExtension : ICapHealthCheckOptionsExtension
    {
        private readonly string _name;
        private readonly HealthStatus? _failureStatus;
        private readonly IEnumerable<string> _tags;
        private readonly TimeSpan? _timeout;

        public PostgreSqlPublishedTableHealthCheckOptionsExtension(string name,
            HealthStatus? failureStatus = null,
            IEnumerable<string> tags = null,
            TimeSpan? timeout = null)
        {
            _name = name;
            _failureStatus = failureStatus;
            _tags = tags;
            _timeout = timeout;
        }
        
        public void AddCheck(IHealthChecksBuilder builder)
        {
            builder.AddCheck<PostgreSqlPublishedTableHealthCheck>(_name, _failureStatus, _tags, _timeout);
        }
    }
}
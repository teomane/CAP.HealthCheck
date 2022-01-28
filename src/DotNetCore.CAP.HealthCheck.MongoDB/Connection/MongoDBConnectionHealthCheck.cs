using System;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP.MongoDB;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DotNetCore.CAP.HealthCheck.MongoDB.Connection
{
    public class MongoDBConnectionHealthCheck : IHealthCheck
    {
        public IMongoDatabase Database { get; }

        public MongoDBConnectionHealthCheck(IOptions<MongoDBOptions> options)
        {
            var client = new MongoClient(options.Value.DatabaseConnection);
            Database = client.GetDatabase(options.Value.DatabaseName);
        }
        
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                using var cursor = await Database.ListCollectionNamesAsync(cancellationToken: cancellationToken);
                await cursor.FirstAsync(cancellationToken);

                return HealthCheckResult.Healthy();
            }
            catch (Exception ex)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
            }
        }
    }
}
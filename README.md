# CAP.HealthCheck

CAP HealthCheck is the health check library of CAP.

# Getting Started
## Nuget

CAP HealthCheck can be installed in your project with the following command.

```
PM> Install-Package DotNetCore.CAP.HealthCheck
```

CAP Health Check supports RabbitMQ Health Check as message queue health check, following package is available to install:

```
PM> Install-Package DotNetCore.CAP.HealthCheck.RabbitMQ
```

CAP Health Check supports PostgreSql，MongoDB as event log storage health check.

```
PM> Install-Package DotNetCore.CAP.HealthCheck.PostgreSql
PM> Install-Package DotNetCore.CAP.HealthCheck.MongoDB
```

## Configuration

```
services.AddHealthChecks()
    .AddCapHealthCheck(setup =>
    {
        # PostgreSql
        setup.AddPostgreSqlConnectionCheck();
        setup.AddPostgreSqlPublishedTableCheck();
        setup.AddPostgreSqlReceivedTableCheck();
        
        # MongoDB
        setup.AddMongoDBConnectionCheck();
        
        # RabbitMQ
        setup.AddRabbitMQConnectionCheck();
    });
```

# Health Check Services
As default, `cap` will be added in tags.

## PostgreSql 
### PostgreSqlConnectionCheck
`AddPostgreSqlConnectionCheck` adds a health check service that tries to access both `Published` and `Received` table of the CAP. Basic select statement is used. If success, health check service returns Healthy result, otherwise Unhealthy. 
Default service name is `cap.postgres.connection`.

### PostgreSqlPublishedTableCheck
`AddPostgreSqlPublishedTableCheck()` adds a health check service that checks for any non _Succeeded_ data in `Published` table.
Shows failed data count and last 100 failed data. Default service name is `cap.postgres.publishedtable`.

### PostgreSqlReceivedTableCheck
`AddPostgreSqlReceivedTableCheck()` adds a health check service that checks for any non _Succeeded_ data in `Received` table.
Shows failed data count and last 100 failed data. Default service name is `cap.postgres.receivedtable`.

## MongoDB
### MongoDBConnectionCheck
`AddMongoDBConnectionCheck` adds a health check service that tries to connect CAP database and list collections. If success, health check service returns Healthy result, otherwise Unhealthy.
Default service name is `cap.mongodb.connection`.

## RabbitMQ
### RabbitMQConnectionCheck
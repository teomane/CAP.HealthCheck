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

```cs
services.AddHealthChecks()
    .AddCapHealthCheck(setup =>
    {
        # PostgreSql
        setup.AddPostgreSqlConnectionCheck();
        setup.AddPostgreSqlPublishedTableCheck();
        setup.AddPostgreSqlReceivedTableCheck();
        
        # MongoDB
        setup.AddMongoDBConnectionCheck();
        setup.AddMongoDBPublishedTableCheck();
        setup.AddMongoDBReceivedTableCheck();
        
        # RabbitMQ
        setup.AddRabbitMQConnectionCheck();
    });
```

# Health Check Services
As default, `cap` will be added in tags.

## Storage Health Checks
### PostgreSql

| Health Check Service               | Default Service Name          | Description                                                                                                                                                                   |
|------------------------------------|-------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `AddPostgreSqlConnectionCheck`     | `cap.postgres.connection`     | Adds a health check service that tries to access both  `Published`  and  `Received`  table of the CAP. Basic select statement is used.                                        |
| `AddPostgreSqlPublishedTableCheck` | `cap.postgres.publishedtable` | Adds a health check service that checks for  _Failed_  data in  `Published`  table using CAP's monitoring API.                                                                | 
| `AddPostgreSqlReceivedTableCheck`  | `cap.postgres.receivedtable`  | Adds a health check service that checks for  _Failed_  data in  `Received`  table using CAP's monitoring API.                                                                 |

### MongoDB

| Health Check Service             | Default Service Name         | Description                                                                                                         |
|----------------------------------|------------------------------|---------------------------------------------------------------------------------------------------------------------|
| `AddMongoDBConnectionCheck`      | `cap.mongodb.connection`     | Adds a health check service that tries to connect CAP database and list collections.                                |
| `AddMongoDBPublishedTableCheck`  | `cap.mongodb.publishedtable` | Adds a health check service that checks for  _Failed_  data in  `Published`  collection using CAP's monitoring API. |
| `AddMongoDBReceivedTableCheck`   | `cap.mongodb.receivedtable`  | Adds a health check service that checks for  _Failed_  data in  `Received`  collection using CAP's monitoring API.  |


## Transport Health Checks
### RabbitMQ

| Health Check Service          | Default Service Name       | Description                                                 |
|-------------------------------|----------------------------|-------------------------------------------------------------|
|  `AddRabbitMQConnectionCheck` |  `cap.rabbitmq.connection` | Adds a healthcheck service that checks RabbitMQ connection. |
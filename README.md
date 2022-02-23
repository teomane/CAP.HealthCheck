<p align="center">
  <img height="140" src="build/logo.png" alt="Sublime's custom image"/>
</p>

# CAP.HealthCheck

The health check library of CAP.

Did this project help you? [You can now buy me a beer 😎🍺.](https://www.buymeacoffee.com/teomane)

[!["You can now buy me a beer 😎🍺."](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://www.buymeacoffee.com/teomane)

# Getting Started
## Nuget

CAP HealthCheck can be installed in your project with the following command.

```
PM> Install-Package DotNetCore.CAP.HealthCheck
```

CAP Health Check supports PostgreSql，MongoDB as event log storage health check.

```
PM> Install-Package DotNetCore.CAP.HealthCheck.PostgreSql
PM> Install-Package DotNetCore.CAP.HealthCheck.MongoDB
```

CAP Health Check supports RabbitMQ Health Check as message queue health check, following package is available to install:

```
PM> Install-Package DotNetCore.CAP.HealthCheck.RabbitMQ
```

## Configuration

```cs
public void ConfigureServices(IServiceCollection services)
{
    // ...

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

    // ...
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // ...

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapCapHealthChecks();
    });
    
    // ...
}
```

Health check service will be available at route `/health-cap` by default.

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

# Sample Applications and Infrastructures

There are two sample applications and each one has own docker compose file to run required infrastructures. **Please check for services and their ports in docker compose files.**

Example command to run docker compose files:
```
docker-compose -f docker-compose-rabbitmq-postgresql.yml up -d
```

Sample applications has a controller named _Test_ with route `api/test`. There are two endpoints for publishing an event with success and fail scenarios. `http://localhost:5000/api/test/publish` publishes an event with successful subscription. `http://localhost:5000/api/test/publish/error` publishes an event with failed subscription. The healthcheck service is available at `http://localhost:5000/health`.

using DotNetCore.CAP.HealthCheck;
using DotNetCore.CAP.HealthCheck.MongoDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Sample.RabbitMQ.MongoDB
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            services.AddCap(options =>
            {
                options.UseMongoDB("mongodb://admin:admin@localhost:27017");
                options.UseRabbitMQ("localhost");
            });

            services.AddHealthChecks()
                .AddCapHealthCheck(setup =>
                { 
                    setup.AddMongoDBConnectionCheck();
                    setup.AddMongoDBPublishedTableCheck();
                    setup.AddMongoDBReceivedTableCheck();
                    setup.AddRabbitMQConnectionCheck();
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    ResponseWriter = ResponseUtil.WriteResponse
                });

                endpoints.MapCapHealthChecks();
            });
        }
    }
}
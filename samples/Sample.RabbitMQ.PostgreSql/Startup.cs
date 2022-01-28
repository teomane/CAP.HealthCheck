using DotNetCore.CAP.HealthCheck;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Sample.RabbitMQ.PostgreSql
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            services.AddCap(options =>
            {
                options.UsePostgreSql("Host=localhost;Port=5432;Database=postgres;Username=admin;Password=admin");
                options.UseRabbitMQ("localhost");
            });

            services.AddHealthChecks()
                .AddCapHealthCheck(setup =>
                {
                    setup.AddPostgreSqlConnectionCheck();
                    setup.AddPostgreSqlPublishedTableCheck();
                    setup.AddPostgreSqlReceivedTableCheck();
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
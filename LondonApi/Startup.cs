﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Formatters;
using LandonApi.Infrastructure;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using LandonApi.Models;
using Microsoft.EntityFrameworkCore;
using LandonApi;
using LondonApi.Filters;

namespace LondonApi
{
    public class Startup
    {
        private readonly int? _httpPort; 
        /*
         The Startup class:
            1. Can optionally include a ConfigureServices method to configure the app's services.
            2. Must include a Configure method to create the app's request processing pipeline.

        ConfigureServices and Configure are called by the runtime when the app starts:
        */
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            /*
             The Startup class constructor accepts dependencies defined by the host. A common use of dependency injection into the Startup class is to inject:
                IHostingEnvironment to configure services by environment.
                IConfiguration to configure the app during startup.
             */
            HostingEnvironment = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
            Configuration = builder.Build();

            // Get the HTTPS port only in development 
            if (env.IsDevelopment())
            {
                var launchJsonConfig = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("Properties\\launchSettings.json")
                    .Build();
                // Getting value of type integer from the path iisSettings:iisExpress:sslPort in the launchSetting.json
                _httpPort = launchJsonConfig.GetValue<int>("iisSettings:iisExpress:sslPort");
            }
        }

        public IHostingEnvironment HostingEnvironment { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // Use an in-memory database for qucik dev and testing
            // Use real database in production
            // Note that because we use in memory database, any data will be lost when the applcaition closes. So we have load test data in startup
            services.AddDbContext<HotelApiContext>(opt => opt.UseInMemoryDatabase());

            /*
             The ConfigureServices method is:
                1. Optional.
                2. Called by the web host before the Configure method to configure the app's services.

             The Configure method is used to specify how the app responds to HTTP requests. The request pipeline is configured by adding middleware components to an IApplicationBuilder instance. 
             IApplicationBuilder is available to the Configure method, but it isn't registered in the service container. 
             */
            if (HostingEnvironment.IsDevelopment())
            {
                // Development configuration

            }
            else
            {
                // Staging/Production configuration

            }

            // Add framework services
            services.AddMvc(opt =>
            {
                // Add JsonExceptionFilter to filters
                opt.Filters.Add(typeof(JsonExceptionFilter));

                // Require HTTPS for all controllers. Redirects clients to encrypted HTTPS connecitons
                opt.SslPort = _httpPort;
                opt.Filters.Add(typeof(RequireHttpsAttribute));

                // OutputFormatters contains all the classes that can format the output of a response like json or text output formatter
                // Grabing a refrence to json output formatter. SIngle makes sure one and only one json formatter is returned. 
                JsonOutputFormatter jsonFormatter = opt.OutputFormatters.OfType<JsonOutputFormatter>().Single();
                // Removing the old Json outputformmater. 
                opt.OutputFormatters.Remove(jsonFormatter);
                // Adding an instance of our ion output by passing a refrence to json formmater that was retrived from the list
                opt.OutputFormatters.Add(new IonOutputFormatter(jsonFormatter));
                // With the above configuration we would see <<  Content-Type →application/ion+json; charset=utf-8  >> in our response. 
            });
            // using lowerclasses
            services.AddRouting(opt => opt.LowercaseUrls = true);

            services.AddApiVersioning(opt =>
            {
                // Using Mediatype versioning
                opt.ApiVersionReader = new MediaTypeApiVersionReader();
                opt.AssumeDefaultVersionWhenUnspecified = true; // Assumes the default version if no other version is specified. 
                opt.ReportApiVersions = true; // Make version reported in header responce.
                opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0); // Default API version is set to 1.0
                opt.ApiVersionSelector = new CurrentImplementationApiVersionSelector(opt);
                // Now you can use ApiVersion("1.0") attribute for each controller
            });

            // This line pull the properties form the info section of the appSettings.json and then creates a new instance of Hotelinfo from those values 
            // Then it wraps the HotelInfo instance in an interface called IOptions and puts that into service container.
            // Once it's in service container it can be injected into contrllers.
            services.Configure<HotelInfo>(Configuration.GetSection("Info"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Order of call matters here. 
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Add some test data in development
                var context = app.ApplicationServices.GetRequiredService<HotelApiContext>();
                AddTestData(context);
            }
            app.UseHsts(opt =>
            {
                // Max age of the header. Client/browser rememebers how long it should rememeber the setting?
                // It's common to set this to very high
                opt.MaxAge(days: 180);
                // Add it not just to root domain but also other subdomains
                opt.IncludeSubdomains();
                // Browser/client can assume the site uses hsts 
                opt.Preload();

                // After all of the above the response will include the hsts header

            });
            app.UseMvc();
        }

        private static void AddTestData(HotelApiContext context)
        {
            // Adding some test data for development
            context.Rooms.Add(new RoomEntity
            {
                Id = Guid.Parse("301df04d-8679-4b1b-ab92-0a586ae53d08"),
                Name = "Oxford Suite",
                Rate = 10119,
            });

            context.Rooms.Add(new RoomEntity
            {
                Id = Guid.Parse("ee2b83be-91db-4de5-8122-35a9e9195976"),
                Name = "Driscoll Suite",
                Rate = 23959
            });

            context.SaveChanges();
        }
    }
}

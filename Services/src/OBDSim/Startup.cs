namespace OBDSim
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Providers;
    using Serilog;
    using Serilog.Context;
    using Serilog.Sinks.Elasticsearch;
    using Swashbuckle.Swagger.Model;
    using System;
    using System.Threading.Tasks;

    public class Startup
    {
        private IConfigurationRoot _configuration;

        public Startup(IHostingEnvironment env)
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            //Log.Logger = new LoggerConfiguration()
            //.ReadFrom.Configuration(_configuration)
            //.CreateLogger();

            Log.Logger  = new LoggerConfiguration()
                        .WriteTo.LiterateConsole()
                        .WriteTo.Elasticsearch(
                            new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                            {
                                IndexFormat = "logstash-index-{0:yyyy.MM}",
                                AutoRegisterTemplate = true
                            })
                        .MinimumLevel.Verbose()
                        .CreateLogger();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            services.AddMvc();
            services.AddScoped<IOBDSimProvider, OBDSimProvider>();

            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "OBDSim API",
                    Description = "API's to get the OBDSim details of an employee",
                    TermsOfService = "None"
                });

                string swaggerPath = _configuration.GetSection("Swagger").GetValue<string>("Path");
                if (string.IsNullOrEmpty(swaggerPath))
                    swaggerPath = @".\\bin\\Debug\\netcoreapp1.0\\OBDSim.xml";

                options.IncludeXmlComments(swaggerPath);
                options.DescribeAllEnumsAsStrings();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();

            app.UseMiddleware<RequestIdMiddleware>();
            app.UseMiddleware<RequestUrlLoggerMiddleware>();
            app.UseMvc();
            app.UseCors("AllowAll");
            app.UseSwagger();
            app.UseSwaggerUi();
        }
    }

    public class RequestUrlLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;

        public RequestUrlLoggerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestUrlLoggerMiddleware>();
        }

        public Task Invoke(HttpContext context)
        {
            _logger.LogInformation("{Method}: {Url}", context.Request.Method, context.Request.Path);
            return _next(context);
        }
    }

    public class RequestIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;

        public RequestIdMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestIdMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            var requestIdFeature = context.Features.Get<IHttpRequestIdentifierFeature>();
            if (requestIdFeature?.TraceIdentifier != null)
            {
                _logger.LogInformation("TraceIdentifier: {TraceIdentifier}", requestIdFeature.TraceIdentifier);
                context.Response.Headers["TraceIdentifier"] = requestIdFeature.TraceIdentifier;
            }

            await _next(context);
        }
    }
}

using CartingService.Messaging;
using CatalogService.API;
using CatalogService.Application.Implementations;
using CatalogService.Application.Interfaces;
using CatalogService.Application.Services;
using CatalogService.Domain.Entities;
using CatalogService.Messaging;
using CatalogService.Persistence.Contexts;
using CatalogService.Persistence.Repositories;
using CatalogService.Persistence.Repositories.Interfaces;
using FluentValidation;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry.Logs;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

ConfigureLogging();
builder.Host.UseSerilog();

var conn = builder.Configuration.GetSection("ConnectionString").Get<string>();

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//                options.UseSqlServer(conn));

builder.Services.AddScoped<ApplicationDbContext>(_ => new ApplicationDbContext(conn));


// Register application services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IItemService, ItemService>();

// Register repository interfaces
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();

builder.Services.AddTransient<IRabbitMqService, RabbitMqService>();

builder.Services.AddHostedService<Worker>();

builder.Services.AddTransient<IValidator<Item>, CreateItemValidator>();

// ConfigureServices method
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Configure method




builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // Configure Swagger to use JWT Bearer authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Keycloak:Authority"];
        options.Audience = "account";
        options.RequireHttpsMetadata = false;
        options.SaveToken = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Keycloak:Authority"]
        };
    });



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("buyer", policy =>
        policy.RequireClaim(JwtClaimTypes.Scope, "read"));
    options.AddPolicy("manager", policy => policy.RequireAssertion(context =>
    {
        var requiredScopes = new[] { "create", "update", "delete", "read" };

        return requiredScopes.All(requiredScope =>
            context.User.HasClaim(c => c.Type == "scope" && c.Value.Contains(requiredScope))
        );
    }));

});

//builder.Services.AddLogging((loggingBuilder) => loggingBuilder
//        .SetMinimumLevel(LogLevel.Debug)
//        .AddOpenTelemetry(options =>
//            options.AddConsoleExporter())
//        );



//builder.Services.AddOpenTelemetry()
//         .WithTracing(builder => builder
//            .AddAspNetCoreInstrumentation()
//            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("EngEx"))
//            .AddOtlpExporter(o =>
//            {
//                o.Endpoint = new Uri("http://localhost:4317");
//            }
//             )
//);
void ConfigureLogging()
{
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile(
            $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
            optional: true)
        .Build();

    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
        .Enrich.WithProperty("Environment", environment)
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}

ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
{
    return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
    };
}

builder.Services.AddOpenTelemetry()
    .WithTracing(builder => builder
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddSqlClientInstrumentation()
        .AddConsoleExporter()
        .AddJaegerExporter()
          .AddSource("Tracing.NET EngEx")
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                .AddService(serviceName: "Tracing.NET EngEx")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication();



app.UseMiddleware<NoCacheMiddleware>();
//app.UseMiddleware<AccessTokenLoggingMiddleware>();
app.UseTokenRefreshMiddleware(builder.Configuration["Keycloak:ClientId"], builder.Configuration["Keycloak:ClientSecret"], builder.Configuration["Keycloak:TokenEndpoint"]);

app.UseAuthorization();
app.MapControllers();

app.Run();

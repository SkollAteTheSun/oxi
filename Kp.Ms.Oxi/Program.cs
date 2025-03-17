using System.Text.Json.Serialization;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Common.HttpClientWrapper;
using Common.RabbitMQClient;
using Common.RabbitMQClient.Config;
using Kp.Ms.Oxi.Config;
using Kp.Ms.Oxi.Context;
using Kp.Ms.Oxi.Extensions;
using Kp.Ms.Oxi.Factories;
using Kp.Ms.Oxi.Interfaces;
using Kp.Ms.Oxi.Models.Options;
using Kp.Ms.Oxi.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<OpenSearchOptions>()
    .Bind(builder.Configuration.GetSection(OpenSearchOptions.Position))
    .ValidateDataAnnotations()
    .ValidateOnStart();

string? bookingConnectionString = builder.Configuration.GetConnectionString("Booking");
if (string.IsNullOrEmpty(bookingConnectionString))
{
    throw new Exception("Connection string 'Booking' not found.");
}
builder.Services.AddDbContext<BookingContext>(options =>
{
    options.UseMySql(bookingConnectionString, ServerVersion.AutoDetect(bookingConnectionString));
}, ServiceLifetime.Transient);
builder.Services.AddDbContextFactory<BookingContext>(
    options =>
    {
        options.UseMySql(bookingConnectionString, ServerVersion.AutoDetect(bookingConnectionString));
    }, ServiceLifetime.Transient
);

builder.Services.AddDbContextFactory<DynamicBookingContext>(options =>
    options.UseMySql(bookingConnectionString, ServerVersion.AutoDetect(bookingConnectionString))
);

builder.Services.AddTransient<IDynamicBookingContextFactory, DynamicBookingContextFactory>();

string? defaultConnectionString = builder.Configuration.GetConnectionString("Db");
if (string.IsNullOrEmpty(defaultConnectionString))
{
    throw new Exception("Connection string 'Db' not found.");
}

builder.Services.AddDbContextFactory<DefaultContext>(
    options => options.UseNpgsql(defaultConnectionString),
    ServiceLifetime.Transient
);

builder.Services
    .AddControllers()
    .AddXmlSerializerFormatters()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); })
    .ConfigureApiBehaviorOptions(options =>
    {
        // To preserve the default behavior, capture the original delegate to call later.
        var builtInFactory = options.InvalidModelStateResponseFactory;

        options.InvalidModelStateResponseFactory = context =>
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<Program>>();

            foreach (var item in context.ModelState)
            {
                if (item.Value.ValidationState != ModelValidationState.Invalid)
                {
                    continue;
                }

                string errors = string.Join(" | ", item.Value.Errors
                    .Select(e => e.ErrorMessage));
                logger.LogError("Model state validation error. Key: {Key}. Errors: {Errors}", item.Key, errors);
            }

            // Invoke the default behavior, which produces a ValidationProblemDetails
            // response.
            // To produce a custom response, return a different implementation of
            // IActionResult instead.
            return builtInFactory(context);
        };
    });

builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option => { option.EnableAnnotations(); });
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddCors(c => c.AddPolicy("cors", opt =>
{
    opt.AllowAnyHeader();
    opt.AllowAnyMethod();
    opt.WithOrigins(builder.Configuration.GetSection("Cors:Urls").Get<string[]>()!);
}));

builder.Services.AddOpenSearch(builder.Configuration);
builder.Services.AddRabbitMQ(builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>());
builder.Services.AddSingleton<IHttpClientWrapper, HttpClientWrapper>();

builder.Services.AddSingleton<LogService>();
builder.Services.AddTransient<RateService>();
builder.Services.AddTransient<ReservationService>();
builder.Services.AddTransient<MessageService>();
builder.Services.AddTransient<TransactionService>();
builder.Services.AddTransient<QuotaService>();
builder.Services.AddTransient<BookingService>();
builder.Services.AddTransient<XmlDeserializer>();

builder.Services.AddOptions<CheckOutProcessingSettings>()
    .Bind(builder.Configuration.GetSection("CheckOutProcessing"));
builder.Services.AddSingleton<PmsService>();
builder.Services.ActivateSingleton<PmsService>();

builder.Services.AddHealthChecks();

var app = builder.Build();
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
if (!app.Environment.IsProduction())
{
    app.UseSwagger(options => { options.RouteTemplate = "/api/swagger/{documentName}/swagger.json"; });
    app.UseSwaggerUI(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/api/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant()
            );
        }

        options.RoutePrefix = "api/swagger";
    });
}

app.MapHealthChecks("/healthz")
    .RequireHost(builder.Configuration.GetSection("MonitoringHosts").Get<string[]>());

app.UseCors("cors");

app.UseAuthorization();

string? token = builder.Configuration.GetSection("Auth:Token").Get<string>();
if (string.IsNullOrEmpty(token))
{
    throw new Exception("Auth token is not configured");
}
app.UseToken(token);

app.MapControllers();

app.Run();
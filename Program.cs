using System.Text.Json.Serialization;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Kp.Ms.Oxi.Config;
using Kp.Ms.Oxi.Context;
using Kp.Ms.Oxi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

string? defaultConnectionString = builder.Configuration.GetConnectionString("Db");
if (string.IsNullOrEmpty(defaultConnectionString))
{
    throw new Exception("Connection string 'Db' not found.");
}
builder.Services.AddDbContext<DefaultContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Db")));


builder.Services
    .AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

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

builder.Services.AddTransient<RateService>();
builder.Services.AddTransient<ReservationService>();
builder.Services.AddTransient<MessageService>();
builder.Services.AddTransient<TransactionService>();

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

app.UseCors("cors");

app.UseAuthorization();

app.MapControllers();

app.Run();
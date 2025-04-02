using BusinessLayer.Extensions;
using FluentValidation;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog loglama entegrasyonu
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();

// OpenAPI (Swagger) desteði
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Agenda API", Version = "v1" });
});

// MemoryCache servisini ekleme (Caching için)
builder.Services.AddMemoryCache();

// FluentValidation servisini ekleme
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);


// AutoMapper ekleme
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Ýþ mantýðý servislerini ekleme (Business katmaný servisleri)
builder.Services.AddBusinessServices();

var app = builder.Build();
// Configure the HTTP request pipeline.


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Agenda API v1"));
}

// Hata yönetimi (Global Exception Handling)
app.UseExceptionHandler("/error");

// Loglama için Serilog middleware
app.UseSerilogRequestLogging();

app.UseAuthorization();
app.MapControllers();


app.Run();

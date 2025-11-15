using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Override configuration sources for testing (disable reloadOnChange)
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.Sources.Clear();
    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    config.AddEnvironmentVariables();
});

builder.Services.AddOpenApi();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection(SmtpSettings.SectionName));
builder.Services.AddSingleton<EmailServiceIoptionMonitor>();

Console.WriteLine($"EmailServiceIoptionMonitor kayitli sayisi: {builder.Services.Count(sd => sd.ServiceType == typeof(EmailServiceIoptionMonitor))}");
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/weatherforecast", (EmailServiceIoptionMonitor svc) =>
{
    svc.SendEmail("a@b.com","o","b");
    return new[] { "ok" };
});

// Test amaçlı: çalışırken config'i elle reload eden endpoint (opsiyonel)
app.MapGet("/reload-config", (IConfiguration configuration) =>
{
    if (configuration is IConfigurationRoot root)
    {
        root.Reload();
        return Results.Ok("Configuration reloaded");
    }
    return Results.Problem("Cannot cast to IConfigurationRoot");
});
app.Run();
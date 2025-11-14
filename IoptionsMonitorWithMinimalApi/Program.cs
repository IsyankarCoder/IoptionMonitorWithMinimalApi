using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.Configure<SmtpSettings>(
     builder.Configuration.GetSection(SmtpSettings.SectionName)
     
);
//builder.Services.AddScoped<EmailService>();
//builder.Services.AddScoped<EmailServiceWithIOptionSnapShot>();
builder.Services.AddTransient<EmailServiceIoptionMonitor>();

// Kayıtlı servisleri logla (debug amaçlı)
int emailServiceMonitorRegistrationCount = 0;
foreach (var sd in builder.Services)
{
    if (sd.ServiceType == typeof(EmailServiceIoptionMonitor))
    {
        emailServiceMonitorRegistrationCount++;
    }
}

Console.WriteLine($"EmailServiceIoptionMonitor kayitli sayisi: {emailServiceMonitorRegistrationCount}");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (//[FromServices] EmailService emailService,
                                               //EmailServiceWithIOptionSnapShot emailServiceWithIOptionSnapShot,
                                               EmailServiceIoptionMonitor emailServiceIoptionMonitor
                                               ) =>
{
      
   // emailService.SendEmail("code.bafra.55@gmail.com","test","test body");
    // emailServiceWithIOptionSnapShot.SendEmail("code.bafra.55@gmail.com","test","test body");
emailServiceIoptionMonitor.SendEmail("code.bafra.55@gmail.com","test","test body");

    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

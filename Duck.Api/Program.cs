using System.IO;
using System.Text.Json.Serialization;
using Duck.Core.DTOs.Duck;
using Duck.Core.DTOs.Quote;
using Duck.Core.Interfaces;
using Duck.Core.Models;
using Duck.Infrastructure.Data;
using Duck.Infrastructure.Data.Seeding;
using Duck.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();

// Hämtar anslutningssträngen från konfigurationen
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Här kontrolleras och skapas databaskatalogen om den inte finns
if (connectionString != null && connectionString.Contains("Data Source="))
{
    var dataSource = connectionString.Replace("Data Source=", "");
    var directory = Path.GetDirectoryName(dataSource);
    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
    {
        Directory.CreateDirectory(directory);
        Console.WriteLine($"Skapade databaskatalog: {directory}");
    }
}



// Registrerar services - Jag använder samma connectionString som tidigare
builder.Services.AddDbContext<DuckContext>(options =>
    options.UseSqlite(connectionString,
        b => b.MigrationsAssembly("Duck.Infrastructure")
    ));
builder.Services.AddScoped<IDuckRepository, DuckRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Jag har ersatt hela min tidigare CORS-konfiguration med denna:
if (builder.Environment.IsDevelopment())
{
    // I utvecklingsmiljö - tillåter jag alla origins
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
    });
}
else
{
    // I produktionsmiljö - använder jag en begränsad policy med portar
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
            builder.WithOrigins("http://localhost:3000",
                    "http://localhost:5173")
                .AllowAnyMethod()
                .AllowAnyHeader());
    });
}


var app = builder.Build();

// Automatisk databasmigration och seedning vid uppstart
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<DuckContext>();
    
    // Kör migrationer
    dbContext.Database.Migrate();
    Console.WriteLine("Databas-migrationer har körts.");
    
    try
    {
        // Kör seedning
        var logger = services.GetRequiredService<ILogger<DuckDataSeeder>>();
        var seeder = new DuckDataSeeder(dbContext, logger); // Referensen blev mycket kortare då vi använder logging.
        seeder.SeedAsync().Wait(); // Använder .Wait() för detta är en synkron kontext?
        Console.WriteLine("Databas har seedats med testdata.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ett fel uppstod under seedning av databasen: {ex.Message}");
    }
}




// Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();



// Jag har ersatt app.UseCors("AllowLocal") med detta:
    app.UseCors();

// DUCK endpoints
app.MapGet("/api/ducks", async (IDuckRepository repository) =>
{
    var ducks = await repository.GetAllDucksAsync();
    return Results.Ok(ducks);
});

app.MapGet("/api/ducks/{id}", async (IDuckRepository repository, int id) =>
{
    var duck = await repository.GetDuckByIdAsync(id);
    return duck is null ? Results.NotFound() : Results.Ok(duck);
});

app.MapPost("/api/ducks", async (IDuckRepository repository, CreateDuckDto createDto) =>
{
    var duck = await repository.CreateDuckAsync(createDto);
    return Results.Created($"/api/ducks/{duck.Id}", duck);
});

app.MapPut("/api/ducks/{id}", async (IDuckRepository repository, int id, UpdateDuckDto updateDto) =>
{
    var duck = await repository.UpdateDuckAsync(id, updateDto);
    return duck is null ? Results.NotFound() : Results.Ok(duck);
});

app.MapDelete("/api/ducks/{id}", async (IDuckRepository repository, int id) =>
{
    var result = await repository.DeleteDuckAsync(id);
    return result ? Results.Ok() : Results.NotFound();
});

// User-Interface funktioner
app.MapGet("/api/user/ducks", async (IDuckRepository repository) =>
{
    var ducks = await repository.GetAllDuckPreviewsAsync();
    return Results.Ok(ducks);
});


app.MapGet("/api/user/ducks/{id}", async (IDuckRepository repository, int id) =>
{
    var duck = await repository.GetDuckPreviewAsync(id);
    return duck is null ? Results.NotFound() : Results.Ok(duck);
});


// QUOTE endpoints
app.MapPost("/api/ducks/{duckId}/quotes", async (
    IDuckRepository repository,
    int duckId,
    CreateQuoteDto createDto) =>
{
    try
    {
        var quote = await repository.AddQuoteAsync(duckId, createDto);
        return Results.Created($"/api/quotes/{quote.Id}", quote);
    }
    catch (ArgumentException ex)
    {
        return Results.NotFound(ex.Message);
    }
});

app.MapPut("/api/quotes/{id}", async (
    IDuckRepository repository,
    int id,
    UpdateQuoteDto updateDto) =>
{
    var quote = await repository.UpdateQuoteAsync(id, updateDto);
    return quote is null ? Results.NotFound() : Results.Ok(quote);
});

app.MapDelete("/api/quotes/{id}", async (IDuckRepository repository, int id) =>
{
    var result = await repository.DeleteQuoteAsync(id);
    return result ? Results.Ok() : Results.NotFound();
});

// Endpoint för att generera ett random quote
app.MapGet("/api/user/ducks/{duckId}/quotes/random", async (
    IDuckRepository repository,
    int duckId,
    [FromQuery] QuoteType type) =>
{
    var quote = await repository.GetRandomQuoteAsync(duckId, type);
    return quote is null 
        ? Results.NotFound($"Inga quotes hittades för anka {duckId} av typ {type}") 
        : Results.Ok(quote);
});

app.Run();


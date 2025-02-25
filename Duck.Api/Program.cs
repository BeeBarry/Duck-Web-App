using System.Text.Json.Serialization;
using Duck.Core.DTOs.Duck;
using Duck.Core.DTOs.Quote;
using Duck.Core.Interfaces;
using Duck.Core.Models;
using Duck.Infrastructure.Data;
using Duck.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registrerar services
builder.Services.AddDbContext<DuckContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Duck.Infrastructure")
        ));
builder.Services.AddScoped<IDuckRepository, DuckRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddCors(options =>

{
    options.AddPolicy("AllowLocal",
        builder => builder
            .WithOrigins("http://localhost:3000",
                "http://localhost:5173"
                ) // Om frontend körs på port 3000
            .AllowAnyMethod()
            .AllowAnyHeader());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowLocal");

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


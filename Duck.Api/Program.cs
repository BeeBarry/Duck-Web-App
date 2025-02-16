using Duck.Core.Interfaces;
using Duck.Core.Models;
using Duck.Infrastructure.Data;
using Duck.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registrerar services
builder.Services.AddDbContext<DuckContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IDuckRepository, DuckRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>

{
    options.AddPolicy("AllowLocal",
        builder => builder
            .WithOrigins("http://localhost:3000") // Om frontend körs på port 3000
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

// Hämta alla ankor
app.MapGet("/api/ducks", async (IDuckRepository repository) =>
{
    var ducks = await repository.GetAllAsync();
    return Results.Ok(ducks);
});

app.MapGet("/api/ducks/{duckId}/quotes/random", async (
    IDuckRepository repository,
    int duckId,
    QuoteType type) =>
{
    var quote = await repository.GetRandomQuoteByTypeAsync(duckId, type);

    if (quote == null)
        return Results.NotFound($" No quote found for duck {duckId} of typ {type}");

    return Results.Ok(quote);

});


app.Run();


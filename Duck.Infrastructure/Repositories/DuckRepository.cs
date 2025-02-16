using Duck.Core.Interfaces;
using Duck.Core.Models;
using Duck.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Duck.Infrastructure.Repositories;

public class DuckRepository : IDuckRepository
{
    private readonly DuckContext _context;

    public DuckRepository(DuckContext context)
    {
        _context = context;
    }

    // Huvudfunktionalitet för användarflödet
    // Hämtar alla ankor med deras tillhörande quotes för att visa i listan
    public async Task<IEnumerable<Core.Models.Duck>> GetAllAsync()
    {
        return await _context.Ducks
            .Include(d => d.Quotes)
            .ToListAsync();
    }

    // Kärnfunktionen för applikationen
    // Hämtar ett slumpmässigt quote av specifik typ från en vald anka
    public async Task<Quote?> GetRandomQuoteByTypeAsync(int duckId, QuoteType type)
    {
        // Räknar bara antalet quotes (mycket snabbare än att ladda alla)
        var quotesCount = await _context.Quotes
            .Where(q => q.DuckId == duckId && q.Type == type)
            .CountAsync();

        if (quotesCount == 0) return null;

        var random = new Random();
        var skipCount = random.Next(quotesCount);

        // Hämtar bara EN quote från databasen
        return await _context.Quotes
            .Where(q => q.DuckId == duckId && q.Type == type)
            .Skip(skipCount)
            .FirstOrDefaultAsync();
    }

    // Administrationsfunktioner för att hantera data
    // Hämtar en specifik anka med alla dess quotes för redigering
    public async Task<Core.Models.Duck?> GetByIdAsync(int id)
    {
        return await _context.Ducks
            .Include(d => d.Quotes)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    // Lägger till en ny anka i systemet
    public async Task<Core.Models.Duck> AddAsync(Core.Models.Duck duck)
    {
        _context.Ducks.Add(duck);
        await _context.SaveChangesAsync();
        return duck;
    }

    // Uppdaterar en existerande ankas information
    public async Task<Core.Models.Duck?> UpdateAsync(Core.Models.Duck duck, bool updateQuotes = false)
    {
        var existingDuck = await GetByIdAsync(duck.Id);
        if (existingDuck == null) return null;

        // Uppdatera endast de properties som skickats med
        if (!string.IsNullOrEmpty(duck.Name))
            existingDuck.Name = duck.Name;
        if (!string.IsNullOrEmpty(duck.Personality))
            existingDuck.Personality = duck.Personality;
        if (!string.IsNullOrEmpty(duck.Motto))
            existingDuck.Motto = duck.Motto;
        if (!string.IsNullOrEmpty(duck.Specialty))
            existingDuck.Specialty = duck.Specialty;

        // Hantera quotes ENDAST om det explicit begärs
        if (updateQuotes && duck.Quotes.Any())
        {
            foreach (var newQuote in duck.Quotes)
            {
                var existingQuote = existingDuck.Quotes
                    .FirstOrDefault(q => q.QuoteId == newQuote.QuoteId);

                if (existingQuote != null)
                {
                    // Uppdatera befintligt quote
                    existingQuote.Content = newQuote.Content;
                    existingQuote.Type = newQuote.Type;
                }
            }
        }

        await _context.SaveChangesAsync();
        return existingDuck;
    }

    // Tar bort en anka och alla dess tillhörande quotes
    public async Task<bool> DeleteAsync(int id)
    {
        // Hämta ankan OCH dess quotes
        var duck = await _context.Ducks
            .Include(d => d.Quotes)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (duck == null) return false;

        // Ta först bort alla quotes
        foreach (var quote in duck.Quotes)
        {
            _context.Quotes.Remove(quote);
        }

        // Ta sedan bort ankan
        _context.Ducks.Remove(duck);
        await _context.SaveChangesAsync();
        return true;
    }
}
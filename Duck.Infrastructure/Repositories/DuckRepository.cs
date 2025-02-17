using Duck.Core.DTOs.Duck;
using Duck.Core.DTOs.Quote;
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

    // Admin funnktioner
    public async Task<IEnumerable<DuckDto>> GetAllDucksAsync()
    {
        var ducks = await _context.Ducks
            .Include(d => d.Quotes)
            .ToListAsync();

        return ducks.Select(d => new DuckDto
        {
            Id = d.Id,
            Name = d.Name,
            Specialty = d.Specialty,
            Personality = d.Personality,
            Motto = d.Motto,
            Quotes = new QuotesByTypeDto
            {
                WiseQuotes = d.Quotes
                    .Where(q => q.Type == QuoteType.Wise)
                    .Select(q => new QuoteDto
                    {
                        Id = q.QuoteId,
                        Content = q.Content,
                        Type = q.Type
                    })
                    .ToList(),
                ComicalQuotes = d.Quotes
                    .Where(q => q.Type == QuoteType.Comical)
                    .Select(q => new QuoteDto
                    {
                        Id = q.QuoteId,
                        Content = q.Content,
                        Type = q.Type
                    })
                    .ToList(),
                DarkQuotes = d.Quotes
                    .Where(q => q.Type == QuoteType.Dark)
                    .Select(q => new QuoteDto
                    {
                        Id = q.QuoteId,
                        Content = q.Content,
                        Type = q.Type
                    })
                    .ToList()
            }
        });
    }

    public async Task<DuckDto?> GetDuckByIdAsync(int id)
    {
        var duck = await _context.Ducks
            .Include(d => d.Quotes)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (duck == null) return null;

        return new DuckDto
        {
            Id = duck.Id,
            Name = duck.Name,
            Specialty = duck.Specialty,
            Personality = duck.Personality,
            Motto = duck.Motto,
            Quotes = new QuotesByTypeDto
            {
                WiseQuotes = duck.Quotes
                    .Where(q => q.Type == QuoteType.Wise)
                    .Select(q => new QuoteDto
                    {
                        Id = q.QuoteId,
                        Content = q.Content,
                        Type = q.Type
                    })
                    .ToList(),
                ComicalQuotes = duck.Quotes
                    .Where(q => q.Type == QuoteType.Comical)
                    .Select(q => new QuoteDto
                    {
                        Id = q.QuoteId,
                        Content = q.Content,
                        Type = q.Type
                    })
                    .ToList(),
                DarkQuotes = duck.Quotes
                    .Where(q => q.Type == QuoteType.Dark)
                    .Select(q => new QuoteDto
                    {
                        Id = q.QuoteId,
                        Content = q.Content,
                        Type = q.Type
                    })
                    .ToList()
            }
        };
    }

    public async Task<DuckDto> CreateDuckAsync(CreateDuckDto createDto)
    {
        var duck = new Core.Models.Duck
        {
            Name = createDto.Name,
            Specialty = createDto.Specialty,
            Personality = createDto.Personality,
            Motto = createDto.Motto
        };

        _context.Ducks.Add(duck);
        await _context.SaveChangesAsync();

        return new DuckDto
        {
            Id = duck.Id,
            Name = duck.Name,
            Specialty = duck.Specialty,
            Personality = duck.Personality,
            Motto = duck.Motto,
            Quotes = new QuotesByTypeDto()
        };
    }

    public async Task<DuckDto?> UpdateDuckAsync(int id, UpdateDuckDto updateDto)
    {
        var duck = await _context.Ducks
            .Include(d => d.Quotes)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (duck == null) return null;

        // Uppdatera endast de fält som inte är null
        if (updateDto.Name != null)
            duck.Name = updateDto.Name;
        if (updateDto.Specialty != null)
            duck.Specialty = updateDto.Specialty;
        if (updateDto.Personality != null)
            duck.Personality = updateDto.Personality;
        if (updateDto.Motto != null)
            duck.Motto = updateDto.Motto;

        await _context.SaveChangesAsync();

        return await GetDuckByIdAsync(id);
    }

    public async Task<bool> DeleteDuckAsync(int id)
    {
        var duck = await _context.Ducks
            .Include(d => d.Quotes)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (duck == null) return false;

        _context.Ducks.Remove(duck);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<QuoteDto> AddQuoteAsync(int duckId, CreateQuoteDto createDto)
    {
        var duck = await _context.Ducks.FindAsync(duckId);
        if (duck == null) 
            throw new ArgumentException($"Ingen anka hittades med ID {duckId}");

        var quote = new Quote
        {
            Content = createDto.Content,
            Type = createDto.Type,
            DuckId = duckId
        };

        _context.Quotes.Add(quote);
        await _context.SaveChangesAsync();

        return new QuoteDto
        {
            Id = quote.QuoteId,
            Content = quote.Content,
            Type = quote.Type
        };
    }

    public async Task<QuoteDto?> UpdateQuoteAsync(int quoteId, UpdateQuoteDto updateDto)
    {
        var quote = await _context.Quotes.FindAsync(quoteId);
        if (quote == null) return null;

        quote.Content = updateDto.Content;
        await _context.SaveChangesAsync();

        return new QuoteDto
        {
            Id = quote.QuoteId,
            Content = quote.Content,
            Type = quote.Type
        };
    }

    public async Task<bool> DeleteQuoteAsync(int quoteId)
    {
        var quote = await _context.Quotes.FindAsync(quoteId);
        if (quote == null) return false;

        _context.Quotes.Remove(quote);
        await _context.SaveChangesAsync();
        return true;
    }
    
    
    // USER funktioner
    
    public async Task<IEnumerable<DuckPreviewDto>> GetAllDuckPreviewsAsync()
    {
        return await _context.Ducks
            .Select(d => new DuckPreviewDto
            {
                Id = d.Id,
                Name = d.Name,
                Specialty = d.Specialty,
                Personality = d.Personality,
                Motto = d.Motto
            })
            .ToListAsync();
    }

    public async Task<DuckPreviewDto?> GetDuckPreviewAsync(int id)
    {
        return await _context.Ducks
            .Where(d => d.Id == id)
            .Select(d => new DuckPreviewDto
            {
                Id = d.Id,
                Name = d.Name,
                Specialty = d.Specialty,
                Personality = d.Personality,
                Motto = d.Motto
            })
            .FirstOrDefaultAsync();
    }

    public async Task<RandomQuoteDto?> GetRandomQuoteAsync(int duckId, QuoteType type)
    {
        // Hämta alla quotes av den specifika typen för den valda ankan
        var quotes = await _context.Quotes
            .Where(q => q.DuckId == duckId && q.Type == type)
            .Include(q => q.Duck)  // Vi behöver ankans namn för responsen
            .ToListAsync();

        if (!quotes.Any()) return null;

        // Välj ett slumpmässigt quote
        var random = new Random();
        var randomQuote = quotes[random.Next(quotes.Count)];

        return new RandomQuoteDto
        {
            Content = randomQuote.Content,
            DuckName = randomQuote.Duck.Name,
            Type = randomQuote.Type
        };
    }
    
}
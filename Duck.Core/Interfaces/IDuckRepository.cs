using Duck.Core.Models;

namespace Duck.Core.Interfaces;

public interface IDuckRepository
{
    // Huvudfunktionalitet
    Task<IEnumerable<Core.Models.Duck>> GetAllAsync();
    Task<Quote?> GetRandomQuoteByTypeAsync(int duckId, QuoteType type);
    
    // Administrationsfunktionalitet
    Task<Core.Models.Duck?> GetByIdAsync(int id);
    Task<Core.Models.Duck> AddAsync(Core.Models.Duck duck);
    Task<Core.Models.Duck?> UpdateAsync(Core.Models.Duck duck, bool updateQuotes = false);
    Task<bool> DeleteAsync(int id);
}
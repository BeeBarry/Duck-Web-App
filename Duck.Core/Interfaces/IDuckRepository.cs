using Duck.Core.DTOs.Duck;
using Duck.Core.DTOs.Quote;
using Duck.Core.Models;

namespace Duck.Core.Interfaces;

public interface IDuckRepository
{
    // Admin funktioner
    Task<IEnumerable<DuckDto>> GetAllDucksAsync();
    Task<DuckDto?> GetDuckByIdAsync(int id);
    Task<DuckDto> CreateDuckAsync(CreateDuckDto createDto);
    Task<DuckDto?> UpdateDuckAsync(int id, UpdateDuckDto updateDto);
    Task<bool> DeleteDuckAsync(int id);
    Task<QuoteDto> AddQuoteAsync(int duckId, CreateQuoteDto createDto);
    Task<QuoteDto?> UpdateQuoteAsync(int quoteId, UpdateQuoteDto updateDto);
    Task<bool> DeleteQuoteAsync(int quoteId);
    
    // User funktioner
    Task<IEnumerable<DuckPreviewDto>> GetAllDuckPreviewsAsync();
    Task<DuckPreviewDto?> GetDuckPreviewAsync(int id);
    Task<RandomQuoteDto?> GetRandomQuoteAsync(int duckId, QuoteType type);
}
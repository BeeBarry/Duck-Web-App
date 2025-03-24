using Duck.Core.Models;

namespace Duck.Core.DTOs.Quote;

public class QuoteDto
{
    public int Id { get; set; }
    public required string Content { get; set; }
    
    public string? Explanation { get; set; }
    public QuoteType Type { get; set; }
}
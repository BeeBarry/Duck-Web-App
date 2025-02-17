using Duck.Core.Models;

namespace Duck.Core.DTOs.Quote;

public class RandomQuoteDto
{
    public string Content { get; set; }
    public string DuckName { get; set; }
    public QuoteType Type { get; set; }
}
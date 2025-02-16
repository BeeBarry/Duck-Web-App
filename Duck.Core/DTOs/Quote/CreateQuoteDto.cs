using Duck.Core.Models;

namespace Duck.Core.DTOs.Quote;

public class CreateQuoteDto
{
    public required string Content { get; set; }
    public required QuoteType Type { get; set; }
}
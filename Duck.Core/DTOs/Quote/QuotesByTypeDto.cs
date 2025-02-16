namespace Duck.Core.DTOs.Quote;

public class QuotesByTypeDto
{
    public List<QuoteDto> WiseQuotes { get; set; } = new();
    public List<QuoteDto> ComicalQuotes { get; set; } = new();
    public List<QuoteDto> DarkQuotes { get; set; } = new();
}
namespace Duck.Core.Models;

public class Quote
{
    public int QuoteId { get; set; }
    public required string Content { get; set; }
    
    public string? Explanation { get; set; }
    public QuoteType Type { get; set; }
    public int DuckId { get; set; }
    public Duck Duck { get; set; } = null!;

}
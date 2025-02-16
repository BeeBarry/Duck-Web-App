using Duck.Core.DTOs.Quote;

namespace Duck.Core.DTOs.Duck;

public class DuckDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Specialty { get; set; }
    public required string Personality { get; set; }
    public required string Motto { get; set; }
    public QuotesByTypeDto Quotes { get; set; } = new();
}
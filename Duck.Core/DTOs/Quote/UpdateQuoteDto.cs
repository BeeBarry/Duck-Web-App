namespace Duck.Core.DTOs.Quote;

public class UpdateQuoteDto
{
    public required string Content { get; set; }
    public string? Explanation { get; set; }
}
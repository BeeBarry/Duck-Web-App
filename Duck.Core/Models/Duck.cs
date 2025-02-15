namespace Duck.Core.Models;

public class Duck
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Specialty { get; set; }
    public required string Personality { get; set; }
    public required string Motto { get; set; }
    public ICollection<Quote> Quotes { get; set; } = new List<Quote>();
}
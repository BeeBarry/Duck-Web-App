namespace Duck.Core.DTOs.Duck;

public class UpdateDuckDto
{
    public required string? Name { get; set; }
    public required string? Specialty { get; set; }
    public required string? Personality { get; set; }
    public required string? Motto { get; set; }
}
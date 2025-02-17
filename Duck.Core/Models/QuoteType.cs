using System.Text.Json.Serialization;

namespace Duck.Core.Models;


[JsonConverter(typeof(JsonStringEnumConverter))]
public enum QuoteType
{
    Wise = 0,
    Comical = 1,
    Dark = 2
}
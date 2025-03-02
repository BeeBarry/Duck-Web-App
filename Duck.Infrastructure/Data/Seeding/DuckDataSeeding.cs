using Duck.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Duck.Infrastructure.Data.Seeding;

public class DuckDataSeeder
{
    private readonly DuckContext _context;
    private readonly ILogger<DuckDataSeeder> _logger;

    public DuckDataSeeder(DuckContext context, ILogger<DuckDataSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        // Säkerställer att databasen är skapad med alla migrations
        await _context.Database.EnsureCreatedAsync();
        
        // Kontrollerar om vi redan har data
        if (await _context.Ducks.AnyAsync())
        {
            _logger.LogInformation("Databasen innehåller redan ankor. Seedning hoppas över.");
            return;
        }

        _logger.LogInformation("Börjar seeda databasen med ankor och citat...");

        try
        {
            await SeedDucksAndQuotesAsync();
            _logger.LogInformation("Databasseedning slutförd framgångsrikt.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ett fel uppstod under seedning av databasen.");
            throw;
        }
    }

    private async Task SeedDucksAndQuotesAsync()
    {
        // Ankor som läggs till
        var daVincii = new Core.Models.Duck
        {
            Name = "Da Vincii",
            Specialty = "UX",
            Personality = "Eccentric & Creative",
            Motto = "Live by the brush, Die by the vision"
        };
        
        var alan = new Core.Models.Duck
        {
            Name = "Alan",
            Specialty = "AI & Algorithm",
            Personality = "Analytical & Curious",
            Motto = "Can machines quack?"
        };
        
        var bruce = new Core.Models.Duck
        {
            Name = "Bruce",
            Specialty = "Cybersecurity",
            Personality = "Serious & Sceptical",
            Motto = "This room is probably bugged"
        };
        
        // Lägger till ankor i databasen
        _context.Ducks.AddRange(daVincii, alan, bruce);
        await _context.SaveChangesAsync();
        
        // Da Vincii quotes
        var daVinciiQuotes = new List<Quote>
        {
            // Wise quotes
            new Quote { Content = "Lyssna på data, men glöm aldrig att det är människor bakom siffrorna.", Type = QuoteType.Wise, DuckId = daVincii.Id },
            new Quote { Content = "En prototyp är värd mer än tusen möten.", Type = QuoteType.Wise, DuckId = daVincii.Id },
            new Quote { Content = "Varje interaktion är en konversation mellan människa och maskin - se till att tala användarens språk.", Type = QuoteType.Wise, DuckId = daVincii.Id },
            
            // Comical quotes
            new Quote { Content = "Min hamburgermeny är vegansk - den innehåller inga onödiga element.", Type = QuoteType.Comical, DuckId = daVincii.Id },
            new Quote { Content = "Min UX-process? Kaffe, panik, inspiration, mer kaffe, och ibland lite design däremellan.", Type = QuoteType.Comical, DuckId = daVincii.Id },
            
            // Dark quotes
            new Quote { Content = "Min design ser bäst ut i Dark Mode - precis som min själ.", Type = QuoteType.Dark, DuckId = daVincii.Id },
            new Quote { Content = "Jag drömmer i wireframes numera. Det är inte frivilligt, det är bara trauma.", Type = QuoteType.Dark, DuckId = daVincii.Id }
        };
        
        // Alan quotes
        var alanQuotes = new List<Quote>
        {
            // Wise quotes
            new Quote { Content = "Maskininlärningsmodeller är bara så bra som datan du matar dem med.", Type = QuoteType.Wise, DuckId = alan.Id },
            new Quote { Content = "Den bästa algoritmen är den som löser problemet, inte den mest eleganta.", Type = QuoteType.Wise, DuckId = alan.Id },
            new Quote { Content = "Artificiell intelligens handlar inte om att ersätta människor, utan om att förstärka mänskliga förmågor.", Type = QuoteType.Wise, DuckId = alan.Id },
            
            // Comical quotes
            new Quote { Content = "Jag skrev en AI som skulle skriva mina skämt, men nu skrattar den bara åt mig.", Type = QuoteType.Comical, DuckId = alan.Id },
            new Quote { Content = "Min AI har blivit så smart att den vägrar köra min dåliga kod.", Type = QuoteType.Comical, DuckId = alan.Id },
            new Quote { Content = "Jag sa till min assistent att planera min dag, nu har den bokat in en semester åt mig.", Type = QuoteType.Comical, DuckId = alan.Id },
            
            // Dark quotes
            new Quote { Content = "AI har redan tagit över, vi märker det bara inte för den är smart nog att låta oss tro att vi fortfarande har kontrollen.", Type = QuoteType.Dark, DuckId = alan.Id },
            new Quote { Content = "Varje bugg vi löser idag är ett steg närmare en algoritm som inte behöver oss imorgon.", Type = QuoteType.Dark, DuckId = alan.Id },
            new Quote { Content = "Den skrämmande frågan är inte när AI kan tänka som en människa, utan när människor börjar tänka som AI.", Type = QuoteType.Dark, DuckId = alan.Id }
        };
        
        // Bruce quotes
        var bruceQuotes = new List<Quote>
        {
            // Wise quotes
            new Quote { Content = "Det finns ingen säkerhet, bara olika nivåer av osäkerhet.", Type = QuoteType.Wise, DuckId = bruce.Id },
            new Quote { Content = "Den svagaste länken i säkerhetskedjan är alltid människan bakom tangentbordet.", Type = QuoteType.Wise, DuckId = bruce.Id },
            new Quote { Content = "En bra säkerhetspraxis är som en försäkring - du uppskattar den inte förrän katastrofen är ett faktum.", Type = QuoteType.Wise, DuckId = bruce.Id },
            
            // Comical quotes
            new Quote { Content = "Mitt lösenord? Det är 'incorrect' så när jag glömmer det säger datorn 'Your password is incorrect'.", Type = QuoteType.Comical, DuckId = bruce.Id },
            new Quote { Content = "Säkerhetsexperter sover med nattlampa. Inte för att de är rädda för mörkret, utan för att mörkret är rädd för dem.", Type = QuoteType.Comical, DuckId = bruce.Id },
            new Quote { Content = "Jag har så många brandväggar att min egen kod behöver visa ID för att få komma in.", Type = QuoteType.Comical, DuckId = bruce.Id },
            
            // Dark quotes
            new Quote { Content = "Det finns bara två typer av företag: de som har blivit hackade och de som inte vet att de har blivit hackade.", Type = QuoteType.Dark, DuckId = bruce.Id },
            new Quote { Content = "Varje gång någon väljer 'password123', vinner hackarna en liten seger.", Type = QuoteType.Dark, DuckId = bruce.Id },
            new Quote { Content = "Vi bygger digitala fästningar medan användarna lämnar bakdörrarna vidöppna.", Type = QuoteType.Dark, DuckId = bruce.Id }
        };
        
        // Lägger till alla quotes i databasen
        _context.Quotes.AddRange(daVinciiQuotes);
        _context.Quotes.AddRange(alanQuotes);
        _context.Quotes.AddRange(bruceQuotes);
        
        await _context.SaveChangesAsync();
    }
}
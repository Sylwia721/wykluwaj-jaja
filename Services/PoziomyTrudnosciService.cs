using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using WykluwajJaja.Models;

namespace WykluwajJaja.Services;

public class PoziomyTrudnosciService
{
    private readonly string _sciezka = Path.Combine(AppContext.BaseDirectory, "poziomy.json");

    private static readonly JsonSerializerOptions Opcje = new() { WriteIndented = true };

    public List<PoziomTrudnosci> Wczytaj()
    {
        if (!File.Exists(_sciezka))
        {
            return Domyslne();
        }

        string json = File.ReadAllText(_sciezka);
        List<PoziomTrudnosci>? poziomy = JsonSerializer.Deserialize<List<PoziomTrudnosci>>(json);

        if (poziomy is null || poziomy.Count == 0)
        {
            return Domyslne();
        }

        return poziomy;
    }

    public void Zapisz(IEnumerable<PoziomTrudnosci> poziomy)
    {
        string json = JsonSerializer.Serialize(poziomy, Opcje);
        File.WriteAllText(_sciezka, json);
    }

    private static List<PoziomTrudnosci> Domyslne()
    {
        return new List<PoziomTrudnosci>
        {
            new() { Nazwa = "Łatwy", LiczbaKlikniec = 10 },
            new() { Nazwa = "Średni", LiczbaKlikniec = 100 },
            new() { Nazwa = "Trudny", LiczbaKlikniec = 1000 }
        };
    }
}

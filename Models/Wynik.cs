using System;

namespace WykluwajJaja.Models;

public class Wynik
{
    public DateTime Data { get; set; }
    public string Poziom { get; set; } = string.Empty;
    public int LiczbaKlikniec { get; set; }
    public TimeSpan Czas { get; set; }
}

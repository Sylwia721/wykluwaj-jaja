# Wykluwaj jaja

Gra desktopowa w WPF (C#) – klikaj jajko, aż wykluje się zwierzątko.

## Autor

Sylwia (GitHub: Sylwia721)

## Temat

Aplikacja WPF: po wybranej liczbie kliknięć jajko pęka i wykluwa się zwierzątko.
Poziom trudności decyduje, ile kliknięć jest potrzebnych.

## Wymagania

- .NET 10
- Windows

## Uruchomienie

1. Sklonuj repozytorium:

   ```
   git clone https://github.com/Sylwia721/wykluwaj-jaja.git
   ```

2. Wejdź do folderu projektu:

   ```
   cd wykluwaj-jaja
   ```

3. Uruchom aplikację:

   ```
   dotnet run
   ```

## Funkcje

- Trzy widoki (zakładki): Gra, Poziomy trudności, Wyniki
- Wybór poziomu trudności i klikanie jajka do wyklucia
- Dodawanie, edycja i usuwanie poziomów trudności (CRUD) z zapisem do pliku JSON
- Lista wyników z wyszukiwaniem i sortowaniem
- Potwierdzanie operacji usunięcia

## Struktura projektu

- `Models` – modele danych
- `ViewModels` – logika aplikacji (MVVM)
- `Views` – widoki XAML
- `Services` – zapis i odczyt danych JSON
- `Commands` – implementacja ICommand
- `Resources` – style i obrazki

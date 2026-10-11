using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using WykluwajJaja.Commands;
using WykluwajJaja.Models;
using WykluwajJaja.Services;

namespace WykluwajJaja.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly PoziomyTrudnosciService _poziomyService = new();

    private PoziomTrudnosci? _wybranyPoziom;
    private string _zdjecie = Zasob("jajo1.png");
    private string _postep = "Wybierz poziom i kliknij Zagraj";
    private bool _gratulacje;
    private bool _graTrwa;
    private int _liczbaKlikniec;
    private DateTime _czasStartu;
    private string _nazwaPoziomu = string.Empty;
    private string _liczbaKlikniecTekst = string.Empty;
    private Wynik? _wybranyWynik;
    private string _filtr = string.Empty;
    private string _poleSortowania = "Data";
    private bool _malejaco;

    public ObservableCollection<PoziomTrudnosci> PoziomyTrudnosci { get; } = new();
    public ObservableCollection<Wynik> Wyniki { get; } = new();
    public ObservableCollection<Wynik> WynikiWidoczne { get; } = new();
    public List<string> PolaSortowania { get; } = new() { "Data", "Poziom", "Kliknięcia", "Czas" };

    public string Zwierze { get; } = Zasob("zwierze.png");

    public RelayCommand ZagrajCommand { get; }
    public RelayCommand KliknijCommand { get; }
    public RelayCommand DodajCommand { get; }
    public RelayCommand ZapiszZmianyCommand { get; }
    public RelayCommand UsunCommand { get; }
    public RelayCommand ZapiszCommand { get; }
    public RelayCommand UsunWynikCommand { get; }

    public MainViewModel()
    {
        ZagrajCommand = new RelayCommand(Zagraj, () => WybranyPoziom is not null && !GraTrwa);
        KliknijCommand = new RelayCommand(Kliknij, () => GraTrwa);
        DodajCommand = new RelayCommand(Dodaj);
        ZapiszZmianyCommand = new RelayCommand(ZapiszZmiany);
        UsunCommand = new RelayCommand(Usun);
        ZapiszCommand = new RelayCommand(ZapiszPoziomy);
        UsunWynikCommand = new RelayCommand(UsunWynik);

        WczytajPoziomy();
    }

    public PoziomTrudnosci? WybranyPoziom
    {
        get => _wybranyPoziom;
        set
        {
            _wybranyPoziom = value;
            OnPropertyChanged();
            ZagrajCommand.RaiseCanExecuteChanged();

            if (value is not null)
            {
                NazwaPoziomu = value.Nazwa;
                LiczbaKlikniecTekst = value.LiczbaKlikniec.ToString();
            }
        }
    }

    public string NazwaPoziomu
    {
        get => _nazwaPoziomu;
        set { _nazwaPoziomu = value; OnPropertyChanged(); }
    }

    public string LiczbaKlikniecTekst
    {
        get => _liczbaKlikniecTekst;
        set { _liczbaKlikniecTekst = value; OnPropertyChanged(); }
    }

    public Wynik? WybranyWynik
    {
        get => _wybranyWynik;
        set { _wybranyWynik = value; OnPropertyChanged(); }
    }

    public string Filtr
    {
        get => _filtr;
        set { _filtr = value; OnPropertyChanged(); OdswiezWyniki(); }
    }

    public string PoleSortowania
    {
        get => _poleSortowania;
        set { _poleSortowania = value; OnPropertyChanged(); OdswiezWyniki(); }
    }

    public bool Malejaco
    {
        get => _malejaco;
        set { _malejaco = value; OnPropertyChanged(); OdswiezWyniki(); }
    }

    public string Zdjecie
    {
        get => _zdjecie;
        private set { _zdjecie = value; OnPropertyChanged(); }
    }

    public string Postep
    {
        get => _postep;
        private set { _postep = value; OnPropertyChanged(); }
    }

    public bool Gratulacje
    {
        get => _gratulacje;
        private set
        {
            _gratulacje = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(TekstStart));
        }
    }

    public bool GraTrwa
    {
        get => _graTrwa;
        private set
        {
            _graTrwa = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(WidocznyStart));
        }
    }

    public bool WidocznyStart => !GraTrwa;
    public string TekstStart => Gratulacje ? "Zagraj jeszcze raz" : "Zagraj";

    public void WczytajPoziomy()
    {
        try
        {
            List<PoziomTrudnosci> poziomy = _poziomyService.Wczytaj();
            PoziomyTrudnosci.Clear();
            foreach (PoziomTrudnosci poziom in poziomy)
            {
                PoziomyTrudnosci.Add(poziom);
            }

            if (WybranyPoziom is null && PoziomyTrudnosci.Count > 0)
            {
                WybranyPoziom = PoziomyTrudnosci[0];
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Nie udało się wczytać poziomów trudności.\n{ex.Message}",
                "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void ZapiszPoziomy()
    {
        try
        {
            _poziomyService.Zapisz(PoziomyTrudnosci);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Nie udało się zapisać poziomów trudności.\n{ex.Message}",
                "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private bool Waliduj(out int liczba)
    {
        liczba = 0;

        if (string.IsNullOrWhiteSpace(NazwaPoziomu))
        {
            MessageBox.Show("Nazwa poziomu nie może być pusta.", "Błąd walidacji",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        if (!int.TryParse(LiczbaKlikniecTekst, out liczba) || liczba <= 0)
        {
            MessageBox.Show("Liczba kliknięć musi być liczbą większą od 0.", "Błąd walidacji",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        return true;
    }

    private void Dodaj()
    {
        if (!Waliduj(out int liczba))
        {
            return;
        }

        PoziomTrudnosci poziom = new() { Nazwa = NazwaPoziomu, LiczbaKlikniec = liczba };
        PoziomyTrudnosci.Add(poziom);
        WybranyPoziom = poziom;
    }

    private void ZapiszZmiany()
    {
        if (WybranyPoziom is null)
        {
            MessageBox.Show("Najpierw zaznacz poziom na liście.", "Informacja",
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        if (!Waliduj(out int liczba))
        {
            return;
        }

        WybranyPoziom.Nazwa = NazwaPoziomu;
        WybranyPoziom.LiczbaKlikniec = liczba;
    }

    private void Usun()
    {
        if (WybranyPoziom is null)
        {
            MessageBox.Show("Najpierw zaznacz poziom na liście.", "Informacja",
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        MessageBoxResult wynik = MessageBox.Show(
            $"Czy na pewno usunąć poziom \"{WybranyPoziom.Nazwa}\"?",
            "Potwierdzenie usunięcia", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (wynik == MessageBoxResult.Yes)
        {
            PoziomyTrudnosci.Remove(WybranyPoziom);
            WybranyPoziom = PoziomyTrudnosci.FirstOrDefault();
        }
    }

    private void OdswiezWyniki()
    {
        IEnumerable<Wynik> wyniki = Wyniki;

        if (!string.IsNullOrWhiteSpace(Filtr))
        {
            wyniki = wyniki.Where(w => w.Poziom.Contains(Filtr, StringComparison.OrdinalIgnoreCase));
        }

        wyniki = PoleSortowania switch
        {
            "Poziom" => Malejaco ? wyniki.OrderByDescending(w => w.Poziom) : wyniki.OrderBy(w => w.Poziom),
            "Kliknięcia" => Malejaco ? wyniki.OrderByDescending(w => w.LiczbaKlikniec) : wyniki.OrderBy(w => w.LiczbaKlikniec),
            "Czas" => Malejaco ? wyniki.OrderByDescending(w => w.Czas) : wyniki.OrderBy(w => w.Czas),
            _ => Malejaco ? wyniki.OrderByDescending(w => w.Data) : wyniki.OrderBy(w => w.Data)
        };

        WynikiWidoczne.Clear();
        foreach (Wynik wynik in wyniki)
        {
            WynikiWidoczne.Add(wynik);
        }
    }

    private void UsunWynik()
    {
        if (WybranyWynik is null)
        {
            MessageBox.Show("Najpierw zaznacz wynik na liście.", "Informacja",
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        MessageBoxResult decyzja = MessageBox.Show(
            $"Czy na pewno usunąć wynik z poziomu \"{WybranyWynik.Poziom}\"?",
            "Potwierdzenie usunięcia", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (decyzja == MessageBoxResult.Yes)
        {
            Wyniki.Remove(WybranyWynik);
            WybranyWynik = null;
            OdswiezWyniki();
        }
    }

    private void Zagraj()
    {
        if (WybranyPoziom is null)
        {
            MessageBox.Show("Najpierw wybierz poziom trudności.", "Informacja",
                MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        _liczbaKlikniec = 0;
        _czasStartu = DateTime.Now;
        Gratulacje = false;
        GraTrwa = true;
        Zdjecie = Zasob("jajo1.png");
        Postep = $"Kliknięcia: 0 / {WybranyPoziom.LiczbaKlikniec}";

        ZagrajCommand.RaiseCanExecuteChanged();
        KliknijCommand.RaiseCanExecuteChanged();
    }

    private void Kliknij()
    {
        if (!GraTrwa || WybranyPoziom is null)
        {
            return;
        }

        _liczbaKlikniec++;
        Postep = $"Kliknięcia: {_liczbaKlikniec} / {WybranyPoziom.LiczbaKlikniec}";

        if (_liczbaKlikniec >= WybranyPoziom.LiczbaKlikniec)
        {
            Wygrana();
            return;
        }

        UstawEtap();
    }

    private void UstawEtap()
    {
        double procent = (double)_liczbaKlikniec / WybranyPoziom!.LiczbaKlikniec;

        if (procent < 1.0 / 3)
        {
            Zdjecie = Zasob("jajo1.png");
        }
        else if (procent < 2.0 / 3)
        {
            Zdjecie = Zasob("jajo2.png");
        }
        else
        {
            Zdjecie = Zasob("jajo3.png");
        }
    }

    private void Wygrana()
    {
        GraTrwa = false;
        Gratulacje = true;
        Zdjecie = Zasob("jajo4.png");

        Wyniki.Add(new Wynik
        {
            Data = DateTime.Now,
            Poziom = WybranyPoziom!.Nazwa,
            LiczbaKlikniec = _liczbaKlikniec,
            Czas = DateTime.Now - _czasStartu
        });

        OdswiezWyniki();

        ZagrajCommand.RaiseCanExecuteChanged();
        KliknijCommand.RaiseCanExecuteChanged();
    }

    private static string Zasob(string plik) => $"pack://application:,,,/Resources/{plik}";
}

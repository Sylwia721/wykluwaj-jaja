using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public ObservableCollection<PoziomTrudnosci> PoziomyTrudnosci { get; } = new();
    public ObservableCollection<Wynik> Wyniki { get; } = new();

    public string Zwierze { get; } = Zasob("zwierze.png");

    public RelayCommand ZagrajCommand { get; }
    public RelayCommand KliknijCommand { get; }

    public MainViewModel()
    {
        ZagrajCommand = new RelayCommand(Zagraj, () => WybranyPoziom is not null && !GraTrwa);
        KliknijCommand = new RelayCommand(Kliknij, () => GraTrwa);

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
        }
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

        ZagrajCommand.RaiseCanExecuteChanged();
        KliknijCommand.RaiseCanExecuteChanged();
    }

    private static string Zasob(string plik) => $"pack://application:,,,/Resources/{plik}";
}

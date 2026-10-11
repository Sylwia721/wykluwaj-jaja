using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using WykluwajJaja.Models;
using WykluwajJaja.Services;

namespace WykluwajJaja.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly PoziomyTrudnosciService _poziomyService = new();

    private PoziomTrudnosci? _wybranyPoziom;

    public ObservableCollection<PoziomTrudnosci> PoziomyTrudnosci { get; } = new();
    public ObservableCollection<Wynik> Wyniki { get; } = new();

    public PoziomTrudnosci? WybranyPoziom
    {
        get => _wybranyPoziom;
        set
        {
            _wybranyPoziom = value;
            OnPropertyChanged();
        }
    }

    public MainViewModel()
    {
        WczytajPoziomy();
    }

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
}

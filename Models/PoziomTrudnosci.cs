using System.ComponentModel;

namespace WykluwajJaja.Models;

public class PoziomTrudnosci : INotifyPropertyChanged
{
    private string _nazwa = string.Empty;
    private int _liczbaKlikniec;

    public string Nazwa
    {
        get => _nazwa;
        set { _nazwa = value; OnPropertyChanged(nameof(Nazwa)); }
    }

    public int LiczbaKlikniec
    {
        get => _liczbaKlikniec;
        set { _liczbaKlikniec = value; OnPropertyChanged(nameof(LiczbaKlikniec)); }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

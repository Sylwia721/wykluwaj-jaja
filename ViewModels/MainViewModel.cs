using System.Collections.ObjectModel;
using WykluwajJaja.Models;

namespace WykluwajJaja.ViewModels;

public class MainViewModel : ViewModelBase
{
    public ObservableCollection<PoziomTrudnosci> PoziomyTrudnosci { get; set; } = new();
    public ObservableCollection<Wynik> Wyniki { get; set; } = new();
}

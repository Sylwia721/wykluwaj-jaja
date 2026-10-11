using System.Windows;

namespace WykluwajJaja.Views;

public partial class OknoPotwierdzenia : Window
{
    public OknoPotwierdzenia(string tresc)
    {
        InitializeComponent();
        Tresc.Text = tresc;
        Owner = Application.Current.MainWindow;
    }

    public static bool Potwierdz(string tresc)
    {
        OknoPotwierdzenia okno = new(tresc);
        return okno.ShowDialog() == true;
    }

    private void Tak(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }

    private void Nie(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WykluwajJaja.Views;

public partial class WidokGry : UserControl
{
    public WidokGry()
    {
        InitializeComponent();
    }

    private void Podskok(object sender, MouseButtonEventArgs e)
    {
        if (PrzyciskJajka.RenderTransform is not TranslateTransform ruch)
        {
            return;
        }

        ruch.BeginAnimation(TranslateTransform.YProperty, null);

        DoubleAnimation animacja = new(0, -18, new Duration(TimeSpan.FromMilliseconds(100)))
        {
            AutoReverse = true,
            FillBehavior = FillBehavior.Stop
        };

        ruch.BeginAnimation(TranslateTransform.YProperty, animacja);
    }
}

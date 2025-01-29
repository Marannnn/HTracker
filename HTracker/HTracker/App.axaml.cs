using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

using HTracker.ViewModels;
using HTracker.Views;
using System.Diagnostics;
namespace HTracker;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) //jestli se program spustil normalne jako desktop aplikace
        {
            var mainViewModel = new MainViewModel(); //vytvorim novou instanci classy MainViewModel (kterou potrebuju) (sama by se vytvorila pozdeji v kodu na radku 28)
            desktop.MainWindow = new MainWindow //vytvorim nove okno desktop aplikace
            {
                DataContext = mainViewModel //ukazuju, ze xaml kod bude binding data brat z instance mainViewModel (vytvoril jsem ho na radku 25)
            };
            Debug.WriteLine("Metoda se spusti");
            mainViewModel.LoadAll(); //spustim custo metodu pres inicializovanou classu MainViewModel
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };
            Debug.WriteLine("NE");
        }

        base.OnFrameworkInitializationCompleted();
    }
}

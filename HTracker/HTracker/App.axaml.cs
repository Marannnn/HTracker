using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

using HTracker.ViewModels;
using HTracker.Views;
using System.Diagnostics;
using HTracker.Model;

namespace HTracker;

public partial class App : Application
{
    private MainViewModel _mainViewModel; //deklaruju mainViewModel
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
            _mainViewModel = new MainViewModel(); //inicializuju _mainViewModel jako novy MainViewModel
            desktop.MainWindow = new MainWindow //vytvorim nove okno desktop aplikace
            {
                DataContext = _mainViewModel //ukazuju, ze xaml kod bude binding data brat z instance mainViewModel (vytvoril jsem ho na radku 25)
            };
            desktop.Exit += OnAppExit; //"kdyz se aplikace vypne, zavolej event handler ,OnAppExit," = "Kdyz se stane desktop.Exit, spust event handler OnAppExit". Kdyby tam bylo -= tak to znamena - "Kdyz se stane tohle, nespoustej tenhle event handler"
            
        }
        
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
    
    private void OnAppExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        _mainViewModel.SaveAll();
    }

}

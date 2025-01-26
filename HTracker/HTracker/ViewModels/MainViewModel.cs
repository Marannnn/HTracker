using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HTracker.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace HTracker.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    ObservableCollection<Habit> HabitCollection { get; } = new ObservableCollection<Habit>(); //kolekce, aby se mohl itemsControl aktualizovat kdykoliv se neco prida nebo odebere
    ObservableCollection<Habit> CompletedHabitCollection { get; } = new();

    [ObservableProperty]
    private uint _currentDay;
    [ObservableProperty]
    private uint _daysRemaining;
    [ObservableProperty]
    private uint _daysCount;

    [ObservableProperty]
    private string _text;
    partial void OnDaysCountChanged(uint value) //tahle metoda je tvorena automaticky diky [ObservableProperty] a spusti se kdykoliv, kdyz se _daysCount zmeni. Ja tady tu metodu jenom "specifikuju" a urcuju ji logiku, co se stane, sama o sobe uz ale existuje
    {
        if (DaysCount > CurrentDay)
        {
            DaysRemaining = DaysCount - CurrentDay;
        }
    }

    [RelayCommand]
    public void AddHabit()
    {
        if (string.IsNullOrWhiteSpace(Text) == false) //jestli uzivatel neco napsal
        {
            HabitCollection.Add(new Habit() //pridam novy element do kolekce, objekt Habit a pridam mu vlastnosti. XAML content pak odkazuje na "content" tohoto objektu
            {
                content = Text,
                isCompleted = false,
            });
            Text = string.Empty;
        }
    }
    public void ResetAll()
    {
        DaysCount = 0;
        DaysRemaining = 0;
        CurrentDay = 0;
        HabitCollection.Clear();
    }

    [RelayCommand]
    public void CheckedHabit()
    {

    }
}

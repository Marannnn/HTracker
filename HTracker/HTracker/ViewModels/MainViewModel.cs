using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HTracker.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace HTracker.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    ObservableCollection<Habit> HabitCollection { get; } = new ObservableCollection<Habit>(); //kolekce, aby se mohl itemsControl aktualizovat kdykoliv se neco prida nebo odebere

    [ObservableProperty]
    private uint _currentDay;
    [ObservableProperty]
    private uint _daysRemaining;
    [ObservableProperty]
    private uint _daysCount;

    [ObservableProperty]
    private string _content;

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
        if (string.IsNullOrWhiteSpace(Content) == false)
        {
            Habit habit = new Habit()
            {
                content = Content 
            };
            HabitCollection.Add(habit); //pridam objekt do kolekce
            Content = string.Empty;
            Debug.WriteLine(habit.content);
        }
    }
    [RelayCommand]
    public void RemoveAll()
    {
        foreach (var habit in HabitCollection)
        {
            HabitCollection.Remove(habit);
        }
        Content = string.Empty;
        CurrentDay = 0u;
        DaysCount = 0u;
        DaysRemaining = 0u;
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HTracker.Model;
using System.Collections.ObjectModel;

namespace HTracker.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    ObservableCollection<Habit> HabitList { get; } = new ObservableCollection<Habit>(); //kolekce, aby se mohl itemsControl aktualizovat kdykoliv se neco prida nebo odebere

    [ObservableProperty]
    private ushort _currentDay;
    [ObservableProperty]
    private ushort _daysCount;
    [ObservableProperty]
    private ushort _daysRemaining;

    [ObservableProperty]
    private string _content;

    [RelayCommand]
    public void AddHabit()
    {
        Habit habit = new Habit()
        {
            content = _content
        };
        HabitList.Add(habit); //pridam objekt do kolekce
    }

}

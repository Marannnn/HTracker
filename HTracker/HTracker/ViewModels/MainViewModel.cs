using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HTracker.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

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
    private string _text;

    public MainViewModel() //konstrukotr se spusti kdyz se inicializuje classa
    {
        DataManager dataManager = new(this);
        dataManager.LoadAll();
    }

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
    [RelayCommand]
    public void ResetAll()
    {
        // //Days
        // DaysCount = 0;
        // DaysRemaining = 0;
        // CurrentDay = 0;
        // HabitCollection.Clear();
        //
        // #region Delete file content
        // string folderPath = Folder();
        // string habitData = Path.Combine(folderPath, "habitData.json");
        // string daysData = Path.Combine(folderPath, "daysData.json");
        // //smazu obsah souboru
        // File.WriteAllText(habitData, string.Empty); 
        // File.WriteAllText(daysData, string.Empty);
        // #endregion
    }

    //TODO - SAVE ON CLOSE . SAVE DATETIME AND UPDATE CURRENT DAY + DAYS REMAINING 
    
    [RelayCommand]
    public void SaveAll()
    {
        // DataManager dataManager = new(DaysCount, DaysRemaining, CurrentDay);
        // dataManager.SaveAll();

   
    }
    [RelayCommand]
    public void LoadAll()
    {
        // string folderPath = Folder();
        // #region Days
        // Dictionary<string, uint> daysDictionary = new();
        // string daysPath = Path.Combine(folderPath, "daysData.json");
        // string jsonString = File.ReadAllText(daysPath);
        // daysDictionary = JsonSerializer.Deserialize<Dictionary<string, uint>>(jsonString);
        // //hardcoded
        // DaysCount = daysDictionary["DaysCount"];
        // DaysRemaining = daysDictionary["DaysRemaining"];
        // CurrentDay = daysDictionary["CurrentDay"];
        // #endregion
        //
        // #region Habits
        // List<Habit> HabitList = new List<Habit>();
        // string habitPath = Path.Combine(folderPath, "habitData.json");
        // jsonString = File.ReadAllText(habitPath);
        // HabitList = JsonSerializer.Deserialize<List<Habit>>(jsonString);
        // foreach (Habit habit in HabitList)
        // {
        //     HabitCollection.Add(habit);
        // }
        // #endregion
    }
}

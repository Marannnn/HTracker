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
    public DateTime trackingStart { get; set; }
    private ObservableCollection<Habit> HabitCollection { get;} = new ObservableCollection<Habit>(); //kolekce, aby se mohl itemsControl aktualizovat kdykoliv se neco prida nebo odebere

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
        //novy zacatek trackingu
        trackingStart = DateTime.Now; 
        CurrentDay = 0;
        DaysRemaining = 0;
    }

    partial void OnCurrentDayChanged(uint value)
    {
        DaysRemaining = DaysCount - CurrentDay;
    }


    #region DataManager Methods
    public void SetHabit(List<Habit> habitList) //vezme list (z DataManager) a postupne prida habity do HabitCollection, aby mohla zustat private
    {
        foreach (Habit habit in habitList)
        {
            HabitCollection.Add(habit);
        }
    }

    public List<Habit> GetHabits()
    {
        List<Habit> habitList = new();
        foreach (Habit habit in HabitCollection)
        {
            habitList.Add(habit);
        }
        return habitList;
    }

    public void UnCheckHabits()
    {
        foreach (Habit habit in HabitCollection)
        {
            habit.isCompleted = false;
        }
    }

    [RelayCommand]
    public void AddHabit()
    {
        if(string.IsNullOrWhiteSpace(Text) == false) //jestli uzivatel neco napsal
        {
            HabitCollection.Add(new Habit() //pridam novy element do kolekce, objekt Habit a pridam mu vlastnosti. XAML content pak odkazuje na "content" tohoto objektu
            {
                content = Text,
                isCompleted = false,
            });
            Text = string.Empty;
        }
    }
    #endregion

    [RelayCommand]
    public void ResetAll()
    {
        //deklarace, vytvoreni slozky
        string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "HTracker"); //odkazuje na slozku "HTracker" v "Dokumenty"
        //Days
        DaysCount = 0;
        DaysRemaining = 0;
        CurrentDay = 0;
        //habits
        HabitCollection.Clear();
        
        //delete file content
        string habitData = Path.Combine(folderPath, "habitData.json");
        string daysData = Path.Combine(folderPath, "daysData.json");
        //smazu obsah souboru
        File.WriteAllText(habitData, string.Empty); 
        File.WriteAllText(daysData, string.Empty);
    }

    //TODO - SAVE ON CLOSE
    
    [RelayCommand]
    public void SaveAll()
    {
        //TODO: save all
    }
}

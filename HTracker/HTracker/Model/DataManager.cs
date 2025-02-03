using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using HTracker.ViewModels;

namespace HTracker.Model;

public class DataManager(MainViewModel mainViewModel)
{
    /// <summary>
    /// FINISHED: save dates(lastOpened, trackingStart), load date and uncheck all habits if last opened in more than 24h
    /// TODO: load dates = change habits and days. Save on app exit
    /// </summary>
    private readonly DateTime _todayDate = DateTime.Now;
    string folderPath;
    private string filePath;
    string jsonString;
    public string Folder() //dostane adresu na custom slozku
    {
        folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "HTracker"); //odkazuje na slozku "HTracker" v "Dokumenty"
        Directory.CreateDirectory(folderPath); //Na te adrese vytvorim slozku
        return folderPath;
    }
        //ULOZIT - v mainViweModel 
    public void SaveAll() //potrebuju vzit curr running data a ulozit je 
    {
        #region Days
        filePath = Path.Combine(Folder(), "daysData.json");
        Dictionary<string, uint> daysDictionary = new Dictionary<string, uint>()
        {
            { "DaysCount", mainViewModel.DaysCount },
            { "DaysRemaining", mainViewModel.DaysRemaining },
            { "CurrentDay", mainViewModel.CurrentDay }
        };
        jsonString = JsonSerializer.Serialize(daysDictionary);
        File.WriteAllText(filePath, jsonString);
        #endregion
        
        #region Habits
        //potrebuju accesnout habity z HabitCollection -> dat do jsonString -> vepsat do souboru
        filePath = Path.Combine(Folder(), "habitsData.json");
        List<Habit> habitList = new List<Habit>();
        habitList = mainViewModel.GetHabits();
        jsonString = JsonSerializer.Serialize(habitList);
        File.WriteAllText(filePath, jsonString);
        #endregion
        
        #region Dates
        filePath = Path.Combine(Folder(), "datesData.json");
        Dictionary<string, DateTime> datesDictionary = new Dictionary<string, DateTime>()
        {
            {"TrackingStart", mainViewModel.trackingStart},
            {"LastOpened", _todayDate}
        };
        jsonString = JsonSerializer.Serialize(datesDictionary);
        File.WriteAllText(filePath, jsonString);
        #endregion
    }
    
    public void LoadAll() //potrebuju vzit ulozena data a dat je do current running
    {
        #region Days
        filePath = Path.Combine(Folder(), "daysData.json");
        jsonString = File.ReadAllText(filePath);
        var daysDictionary = JsonSerializer.Deserialize<Dictionary<string, uint>>(jsonString);
        if (daysDictionary != null)
        {
            mainViewModel.DaysCount = daysDictionary["DaysCount"];
            mainViewModel.DaysRemaining = daysDictionary["DaysRemaining"];
            mainViewModel.CurrentDay = daysDictionary["CurrentDay"];
        }
        #endregion
        
        #region Habits
        filePath = Path.Combine(Folder(), "habitsData.json"); //adresa souboru
        jsonString = File.ReadAllText(filePath); //prevedu do .json do stringu
        var habitList = JsonSerializer.Deserialize<List<Habit>>(jsonString); //priradim objekty .json do habitList
        mainViewModel.SetHabit(habitList); //spusti se metoda SetHabit v MainViewModel -> ta prida obsah habitList do jeji ObservableCollection HabitCollection, abych vlastne primo nepracoval s HabitCollection(aby nemusela byt public)
        #endregion
        
        #region Dates
        //precist data -> lastOpened, jestli rozdil napsaneho a dnesniho data je vetsi nez 24h --> reset all habit checks
        TimeSpan timeDifference;
        filePath = Path.Combine(Folder(), "datesData.json");
        jsonString = File.ReadAllText(filePath);
        var datesDictionary = JsonSerializer.Deserialize<Dictionary<string, DateTime>>(jsonString);
        timeDifference = datesDictionary["LastOpened"] - _todayDate; //ve dnech, rozdil mezi starym zapsanym datem a dnesnim
        if (Math.Abs(timeDifference.Days) > 1) //pokud je rozdil (v absolutni hodnote) ve dnech vetsi nez 1. (pokud to naposledy otevrel 24h vcera)
        {
            mainViewModel.UnCheckHabits();
        }
        
        #endregion
    }
}
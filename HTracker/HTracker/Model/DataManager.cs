using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using HTracker.ViewModels;

namespace HTracker.Model;

public class DataManager(MainViewModel mainViewModel)
{
    private readonly DateTime _lastOpened = DateTime.Now;
    string folderPath;
    private string filePath;
    string jsonString;
    public string Folder() //dostane adresu na custom slozku
    {
        string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "HTracker"); //odkazuje na slozku "HTracker" v "Dokumenty"
        Directory.CreateDirectory(folderPath); //Na te adrese vytvorim slozku
        return folderPath;
    }
        //ULOZIT - v mainViweModel 
    public void SaveAll() //potrebuju vzit curr running data a ulozit je 
    {
        #region Days
        filePath = Path.Combine(Folder(), "daysData.json");
        uint dateTime = Convert.ToUInt32(DateTime.Now.ToString("yyyyMMdd"));
        Dictionary<string, uint> daysDictionary = new Dictionary<string, uint>()
        {
            {"Datetime", dateTime},
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
            {"LastOpened", _lastOpened}
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
        
        #endregion
    }
}
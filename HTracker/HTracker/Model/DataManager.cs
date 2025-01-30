using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using HTracker.ViewModels;

namespace HTracker.Model;

public class DataManager(MainViewModel mainViewModel)
{
    string folderPath;
    string jsonString;
    public string Folder() //dostane adresu na custom slozku
    {
        string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "HTracker"); //odkazuje na slozku "HTracker" v "Dokumenty"
        Directory.CreateDirectory(folderPath); //Na te adrese vytvorim slozku
        return folderPath;
    }
        //ULOZIT - v mainViweModel 
    public void SaveAll()
    {
        #region Days
        folderPath = Path.Combine(Folder(), "daysData.json");
        Dictionary<string, uint> daysDictionary = new Dictionary<string, uint>()
        {
            { "DaysCount", mainViewModel.DaysCount },
            { "DaysRemaining", mainViewModel.DaysRemaining },
            { "CurrentDay", mainViewModel.CurrentDay }
        };
        jsonString = JsonSerializer.Serialize(daysDictionary);
        File.WriteAllText(folderPath, jsonString);
        #endregion
    }

    public void LoadAll()
    {
        #region Days
        folderPath = Path.Combine(Folder(), "daysData.json");
        jsonString = File.ReadAllText(folderPath);
        var daysDictionary = JsonSerializer.Deserialize<Dictionary<string, uint>>(jsonString);
        if (daysDictionary != null)
        {
            mainViewModel.DaysCount = daysDictionary["DaysCount"];
            mainViewModel.DaysRemaining = daysDictionary["DaysRemaining"];
            mainViewModel.CurrentDay = daysDictionary["CurrentDay"];
        }
        #endregion
        #region Habits
        folderPath = Path.Combine(Folder(), "habitsData.json");
        jsonString = File.ReadAllText(folderPath);
        var habitsDictionary = JsonSerializer.Deserialize<Dictionary<string, uint>>(jsonString);
        //TODO: 
        mainViewModel.
        #endregion
    }

}
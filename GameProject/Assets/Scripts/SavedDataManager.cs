using UnityEngine;
using System.IO;

[System.Serializable]
public class GameData
{
    // This class stores a copy of registry entries to be saved/loaded from disk.
    public string JoystickScreenPosition = Constants.LEFT;

    public float MusicVolume = 0.8f;
    public float SFXVolume = 0.8f;
}

public class SavedDataManager
{
    // Used to save/load content from a save location - expand to save game progress.
    private string SaveFileLocation = Application.persistentDataPath;

    private string SaveFileName = "GameData.json";
    private string SavePath = "";

    public SavedDataManager()
    {
        SavePath = Path.Combine(SaveFileLocation, SaveFileName);
    }

    public void Load()
    {
        // Loads data from disk, or uses defaults if save not found.
        GameData LoadedData;
        if (File.Exists(SavePath))
        {
            string dataToLoad = "";
            using (FileStream stream = new FileStream(SavePath, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    dataToLoad = reader.ReadToEnd();
                }
            }
            LoadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            Registry.JoystickScreenPosition = LoadedData.JoystickScreenPosition;
            Registry.MusicVolume = LoadedData.MusicVolume;
            Registry.SFXVolume = LoadedData.SFXVolume;
        }
    }

    public void Save()
    {
        // Saves data to disk.
        Directory.CreateDirectory(SaveFileLocation);

        GameData SavedData = new GameData();
        SavedData.JoystickScreenPosition = Registry.JoystickScreenPosition;
        SavedData.MusicVolume = Registry.MusicVolume;
        SavedData.SFXVolume = Registry.SFXVolume;

        string SerialisedGameData = JsonUtility.ToJson(SavedData, true);

        using (FileStream stream = new FileStream(SavePath, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(SerialisedGameData);
            }
        }
    }
}
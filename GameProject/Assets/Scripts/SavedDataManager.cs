using UnityEngine;
using System.IO;

[System.Serializable]
public class GameData
{
    public string JoystickScreenPosition = Constants.LEFT;
    public float MusicVolume = 0.8f;
    public float SFXVolume = 0.8f;
}

public class SavedDataManager
{
    private string SaveFileLocation = Application.persistentDataPath;
    private string SaveFileName = "GameData.json";
    private string SavePath = "";

    public SavedDataManager()
    {
        SavePath = Path.Combine(SaveFileLocation, SaveFileName);
    }

    public void Load()
    {
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
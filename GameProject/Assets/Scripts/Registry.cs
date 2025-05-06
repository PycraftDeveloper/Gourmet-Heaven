using System.Collections.Generic;
using UnityEngine;

// This class stores a shared collection of variables under a global namespace "Registry".
public static class Registry
{
    public static string CurrentSceneName;

    public static string JoystickScreenPosition = Constants.LEFT;

    public static bool InGameLevel = false;
    public static bool GameInBackground = false;

    public static int LevelNumber = Constants.LEVEL_ONE;
    public static int PlayerScore = 0;

    public static float GameTimeDelta = 0;
    public static float MusicVolume = 0.8f;
    public static float SFXVolume = 0.8f;
    public static float LevelRunTime = 0;

    public static GameManager GameManagerObject = null;
    public static LevelManager LevelManagerObject = null;
    public static Player PlayerObject = null;
    public static Joystick JoystickObject = null;
    public static List<GameObject> Customers = new List<GameObject>();
    public static UIManager UIManagerObject = null;

    public static LevelCustomiser LevelCustomiserObject = new LevelCustomiser();

    public static bool BunsMGTutorialShown = false;
    public static bool PhoMGTutorialShown = false;
    public static bool RiceMGTutorialShown = false;
    public static bool SushiMGTutorialShown = false;

    public static int NotInTutorialScreenTimeModifier = 1;
}
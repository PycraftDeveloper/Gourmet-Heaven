using System.Collections.Generic;
using UnityEngine;

public static class Registry
{
    public static string CurrentSceneName;
    public static string JoystickScreenPosition = Constants.LEFT;

    public static bool InGameLevel = false;

    public static int LevelNumber = Constants.LEVEL_ONE;

    public static float GameTimeDelta = 0;
    public static float MusicVolume = 0.8f;
    public static float SFXVolume = 0.8f;

    public static GameManager GameManagerObject = null;
    public static LevelManager LevelManagerObject = null;
    public static Player PlayerObject = null;
    public static Joystick JoystickObject = null;
    public static List<GameObject> Customers = new List<GameObject>();
}
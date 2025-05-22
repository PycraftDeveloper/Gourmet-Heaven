using System.Collections.Generic;
using UnityEngine;

// This class stores a shared collection of variables under a global namespace "Registry".
public static class Registry
{
    public static string CurrentSceneName;

    public static string JoystickScreenPosition = Constants.LEFT;

    public static bool GameInBackground = false; // Used to determine when to render the pause menu blurred background

    public static int LevelNumber = Constants.LEVEL_ONE; // Stores the level number for the currently played game level
    public static int PlayerScore = 0;

    public static float GameTimeDelta = 0; // Stores 'Time.deltaTime' for the game level, allowing the game to be paused or sped up as needed.
    public static float MusicVolume = 0.8f; // Stores the current music volume (as a percentage).
    public static float SFXVolume = 0.8f; // Stores the current sound effect volume (as a percentage).
    public static float LevelRunTime = 0; // Stores how many seconds the game has been running for (in seconds).

    // Stores objects by reference for use across the project.
    public static GameManager GameManagerObject = null;

    public static LevelManager LevelManagerObject = null;
    public static Player PlayerObject = null;
    public static Joystick JoystickObject = null;
    public static List<ForegroundCustomer> ForegroundCustomers = new List<ForegroundCustomer>();
    public static List<BackgroundCustomer> BackgroundCustomers = new List<BackgroundCustomer>();
    public static UIManager UIManagerObject = null;

    public static LevelCustomiser LevelCustomiserObject = new LevelCustomiser(); // Used to customise the registry for the different game levels

    // Used to keep track of when to show the tutorial screen in game. (Note: could be saved to disk so persists across restarts).
    public static bool BunsMGTutorialShown = false;

    public static bool PhoMGTutorialShown = false;
    public static bool RiceMGTutorialShown = false;
    public static bool SushiMGTutorialShown = false;

    public static int NotInTutorialScreenTimeModifier = 1; // Used to stop time passing when reading the tutorial so player not rushed.

    public static bool IntroSequencePlayed = false;
}
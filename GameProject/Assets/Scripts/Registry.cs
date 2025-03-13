public static class Registry
{
    public static bool PlayerExists = false;
    public static bool JoystickExists = false;
    public static bool LevelManagerExists = false;
    public static bool AudioManager = false;

    public static bool InGameLevel = false;

    public static float GameTimeDelta = 0;

    public static string CurrentLocation = Constants.KITCHEN;

    public static string JoystickScreenPosition = Constants.LEFT;

    public static float MusicVolume = 0.8f;
    public static float SFXVolume = 0.8f;

    public static string PreviousMenu = Constants.MAIN_MENU;

    public static bool GamePaused = false;
}
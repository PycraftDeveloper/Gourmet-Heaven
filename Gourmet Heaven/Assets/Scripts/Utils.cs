// This program is used to set-up the registry uniquely for each of the game levels.

public class LevelCustomiser
{
    public void SetupLevelOne()
    {
        Registry.LevelRunTime = Constants.LEVEL_ONE_DURATION;
        Registry.LevelNumber = Constants.LEVEL_ONE;
    }

    public void SetupLevelTwo()
    {
        Registry.LevelRunTime = Constants.LEVEL_TWO_DURATION;
        Registry.LevelNumber = Constants.LEVEL_TWO;
    }
}
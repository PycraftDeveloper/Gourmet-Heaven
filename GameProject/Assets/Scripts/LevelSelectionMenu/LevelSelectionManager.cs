using UnityEngine;

public class LevelSelectionMenuManagerScript : MonoBehaviour
{
    public void OnLevelOneButtonClick()
    {
        Registry.LevelNumber = Constants.LEVEL_ONE;
        Registry.GameManagerObject.ChangeScene(Constants.KITCHEN);
    }

    public void OnLevelTwoButtonClick()
    {
        Registry.LevelNumber = Constants.LEVEL_TWO;
        Registry.GameManagerObject.ChangeScene(Constants.KITCHEN);
    }

    public void OnBackButtonClick()
    {
        Registry.GameManagerObject.ChangeScene();
    }
}
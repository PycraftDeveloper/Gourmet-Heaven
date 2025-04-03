using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public void OnPlayAgainButtonClick()
    {
        if (Registry.LevelNumber == Constants.LEVEL_ONE)
        {
            Registry.LevelCustomerObject.SetupLevelOne();
        }
        else
        {
            Registry.LevelCustomerObject.SetupLevelTwo();
        }
        Registry.GameManagerObject.ChangeScene(Constants.KITCHEN);
    }

    public void OnMainMenuButtonClick()
    {
        Registry.GameManagerObject.ChangeScene(Constants.MAIN_MENU);
    }

    public void OnQuitButtonClick()
    {
        Registry.GameManagerObject.QuitGame();
    }
}
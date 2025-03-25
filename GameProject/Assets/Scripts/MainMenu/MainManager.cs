using UnityEngine;

public class MainMenuManagerScript : MonoBehaviour
{
    public void Start()
    {
        Application.targetFrameRate = Mathf.Max(60, (int)Screen.currentResolution.refreshRateRatio.value);
    }

    public void OnPlayButtonClick()
    {
        Registry.GameManagerObject.ChangeScene(Constants.LEVEL_SELECTION_MENU);
    }

    public void OnShopButtonClick()
    {
        Registry.GameManagerObject.ChangeScene(Constants.SHOP_MENU);
    }

    public void OnOptionsButtonClick()
    {
        Registry.GameManagerObject.ChangeScene(Constants.OPTIONS_MENU);
    }

    public void OnCreditsButtonClick()
    {
        Registry.GameManagerObject.ChangeScene(Constants.CREDITS_MENU);
    }

    public void OnQuitButtonClick()
    {
        Registry.GameManagerObject.QuitGame();
    }
}
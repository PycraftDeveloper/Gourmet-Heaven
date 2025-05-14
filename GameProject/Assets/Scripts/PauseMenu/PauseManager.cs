using UnityEngine;

public class PauseMenuManagerScript : MonoBehaviour
{
    public void OnResumeButtonClick()
    {
        Registry.GameManagerObject.ChangeScene();
    }

    public void OnOptionsButtonClick()
    {
        Registry.GameManagerObject.ChangeScene(Constants.OPTIONS_MENU);
    }

    public void OnMainMenuButtonClick()
    {
        Registry.GameManagerObject.ChangeScene(Constants.MAIN_MENU);
    }

    public void OnQuitButtonClick()
    {
        Registry.GameManagerObject.QuitGame();
    }

    public void Update() // Used to allow keyboard interaction for Windows builds.
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnResumeButtonClick();
        }
    }
}
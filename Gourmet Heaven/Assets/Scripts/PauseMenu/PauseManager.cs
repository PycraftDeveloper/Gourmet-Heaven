using UnityEngine;

public class PauseMenuManagerScript : MonoBehaviour
{
    public Canvas MenuCanvas;

    public void Start()
    {
        MenuCanvas.worldCamera = Camera.main;
        MenuCanvas.sortingLayerName = "UI";
    }

    public void OnResumeButtonClick()
    {
        Registry.GamePaused = false;
        Registry.CoreGameInfrastructureObject.SFXSource.PlayOneShot(Registry.CoreGameInfrastructureObject.ButtonClickSound);
        Registry.CoreGameInfrastructureObject.CloseMenu();
    }

    public void OnSettingsButtonClick()
    {
        Registry.CoreGameInfrastructureObject.SFXSource.PlayOneShot(Registry.CoreGameInfrastructureObject.ButtonClickSound);
        Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.SETTINGS_MENU);
    }

    public void OnMainMenuButtonClick()
    {
        Registry.CoreGameInfrastructureObject.SFXSource.PlayOneShot(Registry.CoreGameInfrastructureObject.ButtonClickSound);
        Registry.CoreGameInfrastructureObject.ChangeScene(Constants.MENU_SCENE);
        Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.MAIN_MENU);
    }

    public void OnQuitButtonClick()
    {
        Registry.CoreGameInfrastructureObject.SFXSource.PlayOneShot(Registry.CoreGameInfrastructureObject.ButtonClickSound);
        Registry.CoreGameInfrastructureObject.QuitGame();
    }

    public void Update() // Used to allow keyboard interaction for Windows builds.
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Registry.GamePaused = false;
            Registry.CoreGameInfrastructureObject.CloseMenu();
        }
    }
}
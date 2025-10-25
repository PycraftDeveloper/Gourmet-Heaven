using UnityEngine;

public class LevelSelectionMenuManagerScript : MonoBehaviour
{
    public Canvas MenuCanvas;

    public void Start()
    {
        MenuCanvas.worldCamera = Camera.main;
    }

    public void OnLevelOneButtonClick()
    {
        Registry.LevelCustomiserObject.SetupLevelOne();
        Registry.CoreGameInfrastructureObject.SFXSource.PlayOneShot(Registry.CoreGameInfrastructureObject.ButtonClickSound);
        if (!Registry.GameTutorialShown)
        {
            Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.GAME_TUTORIAL_MENU);
        }
        else
        {
            Registry.CoreGameInfrastructureObject.ChangeScene(Constants.GAME_SCENE);
        }
    }

    public void OnLevelTwoButtonClick()
    {
        Registry.LevelCustomiserObject.SetupLevelTwo();
        Registry.CoreGameInfrastructureObject.SFXSource.PlayOneShot(Registry.CoreGameInfrastructureObject.ButtonClickSound);
        if (!Registry.GameTutorialShown)
        {
            Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.GAME_TUTORIAL_MENU);
        }
        else
        {
            Registry.CoreGameInfrastructureObject.ChangeScene(Constants.GAME_SCENE);
        }
    }

    public void OnBackButtonClick()
    {
        Registry.CoreGameInfrastructureObject.SFXSource.PlayOneShot(Registry.CoreGameInfrastructureObject.ButtonClickSound);
        Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.PREVIOUS_MENU);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.PREVIOUS_MENU);
        }
    }
}
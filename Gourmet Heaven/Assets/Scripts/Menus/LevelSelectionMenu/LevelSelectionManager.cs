using UnityEngine;

public class LevelSelectionMenuManagerScript : MonoBehaviour
{
    [Header("UI")]
    public Canvas MenuCanvas;

    public void Start()
    {
        MenuCanvas.worldCamera = Camera.main;
    }

    public void OnLevelOneButtonClick()
    {
        Registry.CGI.SetupLevelOne();

        Registry.CGI.PlayClickSound();
        Registry.InGame = true;
        if (!Registry.GameTutorialShown)
        {
            Registry.CGI.ChangeMenu(Constants.GAME_TUTORIAL_MENU);
        }
        else
        {
            Registry.CGI.ChangeScene(Constants.GAME_SCENE);
        }
    }

    public void OnLevelTwoButtonClick()
    {
        Registry.CGI.SetupLevelTwo();

        Registry.CGI.PlayClickSound();
        Registry.InGame = true;
        if (!Registry.GameTutorialShown)
        {
            Registry.CGI.ChangeMenu(Constants.GAME_TUTORIAL_MENU);
        }
        else
        {
            Registry.CGI.ChangeScene(Constants.GAME_SCENE);
        }
    }

    public void OnBackButtonClick()
    {
        Registry.CGI.PlayClickSound();
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(gameObject);
        }
    }
}
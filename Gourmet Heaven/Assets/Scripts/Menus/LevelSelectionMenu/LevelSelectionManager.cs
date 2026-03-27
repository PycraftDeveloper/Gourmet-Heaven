using UnityEngine;

public class LevelSelectionMenuManagerScript : MonoBehaviour
{
    [Header("UI")]
    public Canvas MenuCanvas;

    [Header("Navigable Menus")]
    public GameObject MainMenuPrefab;

    public void Start()
    {
        MenuCanvas.worldCamera = Camera.main;
    }

    public void OnLevelOneButtonClick()
    {
        Registry.CGI.SetupLevelOne();

        Registry.CGI.SFXSource.PlayOneShot(Registry.CGI.ButtonClickSound);
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

        Registry.CGI.SFXSource.PlayOneShot(Registry.CGI.ButtonClickSound);
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
        Registry.CGI.SFXSource.PlayOneShot(Registry.CGI.ButtonClickSound);
        Instantiate(MainMenuPrefab);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Instantiate(MainMenuPrefab);
            Destroy(gameObject);
        }
    }
}
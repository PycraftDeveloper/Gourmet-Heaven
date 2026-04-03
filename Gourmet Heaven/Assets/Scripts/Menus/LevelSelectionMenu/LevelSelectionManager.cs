using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionMenuManagerScript : MonoBehaviour
{
    [Header("UI")]
    public Canvas MenuCanvas;

    [Header("Navigable Menus")]
    public GameObject GameTutorialMenuPrefab;

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
            Instantiate(GameTutorialMenuPrefab);
            Destroy(this.gameObject);
        }
        else
        {
            SceneManager.LoadScene(Constants.GAME_SCENE);
        }
    }

    public void OnLevelTwoButtonClick()
    {
        Registry.CGI.SetupLevelTwo();

        Registry.CGI.PlayClickSound();
        Registry.InGame = true;
        if (!Registry.GameTutorialShown)
        {
            Instantiate(GameTutorialMenuPrefab);
            Destroy(this.gameObject);
        }
        else
        {
            SceneManager.LoadScene(Constants.GAME_SCENE);
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
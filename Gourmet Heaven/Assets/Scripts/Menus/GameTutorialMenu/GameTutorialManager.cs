using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTutorialManager : MonoBehaviour
{
    public Canvas MenuCanvas;

    public void Start()
    {
        MenuCanvas.worldCamera = Camera.main;
    }

    public void OnContinueButtonClick()
    {
        Registry.CGI.PlayClickSound();
        Camera.main.enabled = false;

        SceneManager.LoadScene(Constants.GAME_SCENE);
    }
}
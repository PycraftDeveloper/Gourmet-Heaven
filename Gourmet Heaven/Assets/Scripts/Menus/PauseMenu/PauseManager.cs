using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManagerScript : MonoBehaviour
{
    [Header("UI")]
    public Canvas MenuCanvas;

    [Header("Background")]
    public RawImage BackgroundImage;

    [Header("Navigable Menus")]
    public GameObject SettingsMenuPrefab;

    public void Start()
    {
        MenuCanvas.worldCamera = Camera.main;
        MenuCanvas.sortingLayerName = "UI";

        // use "Registry.GameManagerObject.FrameTexture;" for no blur
        // use "Registry.GameManagerObject.FrameTexture;" for blur
        // use transparency if you want the blur to appear to 'fade in'
        BackgroundImage.texture = Registry.CGI.BlurredFrameTexture;
    }

    public void OnResumeButtonClick()
    {
        Registry.GamePaused = false;
        Registry.CGI.PlayClickSound();
        Destroy(gameObject);
    }

    public void OnSettingsButtonClick()
    {
        Registry.CGI.PlayClickSound();
        Instantiate(SettingsMenuPrefab);
    }

    public void OnMainMenuButtonClick()
    {
        Registry.CGI.PlayClickSound();
        SceneManager.LoadScene(Constants.MENU_SCENE);
    }

    public void OnQuitButtonClick()
    {
        Registry.CGI.QuitGame();
    }

    public void Update() // Used to allow keyboard interaction for Windows builds.
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Registry.GamePaused = false;
            Destroy(gameObject);
        }
    }
}
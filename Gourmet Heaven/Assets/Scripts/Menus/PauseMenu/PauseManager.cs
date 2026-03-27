using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManagerScript : MonoBehaviour
{
    [Header("UI")]
    public Canvas MenuCanvas;

    [Header("Background")]
    public RawImage BackgroundImage;

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
        Registry.CGI.SFXSource.PlayOneShot(Registry.CGI.ButtonClickSound);
        Registry.CGI.CloseMenu();
    }

    public void OnSettingsButtonClick()
    {
        Registry.CGI.SFXSource.PlayOneShot(Registry.CGI.ButtonClickSound);
        Registry.CGI.ChangeMenu(Constants.SETTINGS_MENU);
    }

    public void OnMainMenuButtonClick()
    {
        Registry.CGI.SFXSource.PlayOneShot(Registry.CGI.ButtonClickSound);
        Registry.CGI.ChangeScene(Constants.MENU_SCENE);
        Registry.CGI.ChangeMenu(Constants.MAIN_MENU);
    }

    public void OnQuitButtonClick()
    {
        Registry.CGI.SFXSource.PlayOneShot(Registry.CGI.ButtonClickSound);
        Registry.CGI.QuitGame();
    }

    public void Update() // Used to allow keyboard interaction for Windows builds.
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Registry.GamePaused = false;
            Registry.CGI.CloseMenu();
        }
    }
}
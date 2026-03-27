using UnityEngine;

public class GameTutorialManager : MonoBehaviour
{
    public Canvas MenuCanvas;

    public void Start()
    {
        MenuCanvas.worldCamera = Camera.main;
    }

    public void OnContinueButtonClick()
    {
        Registry.CGI.SFXSource.PlayOneShot(Registry.CGI.ButtonClickSound);
        Camera.main.enabled = false;
        Registry.CGI.ChangeScene(Constants.GAME_SCENE);

        Registry.CGI.musicSource.Stop();
    }
}
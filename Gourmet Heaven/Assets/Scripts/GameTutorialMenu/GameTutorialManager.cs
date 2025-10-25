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
        Registry.CoreGameInfrastructureObject.SFXSource.PlayOneShot(Registry.CoreGameInfrastructureObject.ButtonClickSound);
        Camera.main.enabled = false;
        Registry.CoreGameInfrastructureObject.ChangeScene(Constants.GAME_SCENE);
    }
}
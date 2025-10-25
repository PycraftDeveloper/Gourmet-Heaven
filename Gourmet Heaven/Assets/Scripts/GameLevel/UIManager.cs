using UnityEngine;

public class UIManager : MonoBehaviour // This class controls the buttons and behaviour of the UI for the game.
{
    private void Awake() // This object is also set to persist between scene changes and is managed instead in the GameManager.
    {
        Registry.UIManagerObject = this;
    }

    public void OnPauseButtonClick()
    {
        Registry.GamePaused = true;
        Registry.CoreGameInfrastructureObject.RenderGameSceneToFrameBuffer();
        Registry.CoreGameInfrastructureObject.SFXSource.PlayOneShot(Registry.CoreGameInfrastructureObject.ButtonClickSound);
        Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.PAUSE_MENU);
    }

    public void OnSceneChanged() // Ensure that the canvas uses the correct world camera when the scenes change (as the camera for the previous scene is destroyed).
    {
        Registry.UIManagerObject.GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
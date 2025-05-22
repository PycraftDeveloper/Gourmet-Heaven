using UnityEngine;

public class LevelSelectionMenuManagerScript : MonoBehaviour
{
    public Canvas TutorialCanvas;
    public Canvas LevelSelectCanvas;

    private void Start()
    {
        LevelSelectCanvas.gameObject.SetActive(true);
    }

    public void OnLevelOneButtonClick()
    {
        LevelSelectCanvas.gameObject.SetActive(false);
        TutorialCanvas.gameObject.SetActive(true);
    }

    public void OnLevelTwoButtonClick()
    {
        Camera.main.enabled = false; // Hide the camera to avoid the black frame as current scene unloads and game scene loads.
        // Could add a loading screen here if the loading took a more significant time. (The camera is never reset to enabled, because the camera is
        // later destroyed and recreated in the new scene).
        Registry.LevelCustomiserObject.SetupLevelTwo();
        Registry.GameManagerObject.ChangeScene(Constants.GAME_LEVEL);
    }

    public void OnContinueButtonClick()
    {
        Camera.main.enabled = false;
        TutorialCanvas.gameObject.SetActive(false);
        Registry.LevelCustomiserObject.SetupLevelOne();
        Registry.GameManagerObject.ChangeScene(Constants.GAME_LEVEL);
    }

    public void OnBackButtonClick()
    {
        Registry.GameManagerObject.ChangeScene();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackButtonClick();
        }
    }
}
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
        Camera.main.enabled = false;
        Registry.LevelCustomiserObject.SetupLevelTwo();
        Registry.GameManagerObject.ChangeScene(Constants.KITCHEN);
    }

    public void OnContinueButtonClick()
    {
        Camera.main.enabled = false;
        TutorialCanvas.gameObject.SetActive(false);
        Registry.LevelCustomiserObject.SetupLevelOne();
        Registry.GameManagerObject.ChangeScene(Constants.KITCHEN);
    }

    public void OnBackButtonClick()
    {
        Registry.GameManagerObject.ChangeScene();
    }
}
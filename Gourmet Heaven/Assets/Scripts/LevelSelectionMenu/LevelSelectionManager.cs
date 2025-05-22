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
        Registry.LevelCustomiserObject.SetupLevelOne();
        if (!Registry.GameTutorialShown)
        {
            TutorialCanvas.gameObject.SetActive(true);
        }
        else
        {
            Camera.main.enabled = false;
            Registry.GameManagerObject.ChangeScene(Constants.GAME_LEVEL);
        }
    }

    public void OnLevelTwoButtonClick()
    {
        Registry.LevelCustomiserObject.SetupLevelTwo();
        if (!Registry.GameTutorialShown)
        {
            TutorialCanvas.gameObject.SetActive(true);
        }
        else
        {
            Camera.main.enabled = false;
            Registry.GameManagerObject.ChangeScene(Constants.GAME_LEVEL);
        }
    }

    public void OnContinueButtonClick()
    {
        Camera.main.enabled = false;
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
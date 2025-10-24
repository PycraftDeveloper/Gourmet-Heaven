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
        Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.ButtonClickSound);
        Registry.LevelCustomiserObject.SetupLevelOne();
        if (!Registry.GameTutorialShown)
        {
            LevelSelectCanvas.gameObject.SetActive(false);
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
        Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.ButtonClickSound);
        Registry.LevelCustomiserObject.SetupLevelTwo();
        if (!Registry.GameTutorialShown)
        {
            LevelSelectCanvas.gameObject.SetActive(false);
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
        Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.ButtonClickSound);
        Camera.main.enabled = false;
        Registry.GameManagerObject.ChangeScene(Constants.GAME_LEVEL);
    }

    public void OnBackButtonClick()
    {
        Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.ButtonClickSound);
        Registry.GameManagerObject.ChangeScene();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Registry.GameManagerObject.ChangeScene();
        }
    }
}
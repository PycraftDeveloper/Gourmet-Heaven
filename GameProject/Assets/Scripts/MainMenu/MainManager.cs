using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManagerScript : MonoBehaviour
{
    public void Start()
    {
        Application.targetFrameRate = Mathf.Max(60, (int)Screen.currentResolution.refreshRateRatio.value);
    }

    public void OnPlayButtonClick()
    {
        SceneManager.LoadScene("LevelSelectionMenu");
    }

    public void OnShopButtonClick()
    {
        SceneManager.LoadScene("ShopMenu");
    }

    public void OnOptionsButtonClick()
    {
        SceneManager.LoadScene("OptionsMenu");
    }

    public void OnCreditsButtonClick()
    {
        SceneManager.LoadScene("CreditsMenu");
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
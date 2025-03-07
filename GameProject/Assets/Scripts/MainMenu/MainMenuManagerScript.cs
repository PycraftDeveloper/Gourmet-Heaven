using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.SceneManagement;

public class MainMenuManagerScript : MonoBehaviour
{
    public void Start()
    {
        Application.targetFrameRate = 120;
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
}
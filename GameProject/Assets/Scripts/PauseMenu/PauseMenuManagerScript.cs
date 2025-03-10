using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManagerScript : MonoBehaviour
{
    public void OnReturnToGameButtonClick()
    {
        if (Registry.CurrentLocation == Constants.RESTAURANT)
        {
            SceneManager.LoadScene("Restaurant");
        }
        else
        {
            SceneManager.LoadScene("Kitchen");
        }
    }

    public void OnMainMenuButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
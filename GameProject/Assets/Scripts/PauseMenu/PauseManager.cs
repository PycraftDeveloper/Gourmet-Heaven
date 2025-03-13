using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManagerScript : MonoBehaviour
{
    public void Start()
    {
        Registry.GamePaused = true;
    }

    public void OnResumeButtonClick()
    {
        Registry.GamePaused = false;
        switch (Registry.PreviousMenu)
        {
            case Constants.IN_GAME:
                if (Registry.CurrentLocation == Constants.RESTAURANT)
                {
                    SceneManager.LoadScene("Restaurant");
                }
                else
                {
                    SceneManager.LoadScene("Kitchen");
                }
                break;

            case Constants.BUNS_MG:
                SceneManager.LoadScene("BunsMG");
                break;

            case Constants.PHO_MG:
                SceneManager.LoadScene("PhoMG");
                break;

            case Constants.RICE_MG:
                SceneManager.LoadScene("RiceMG");
                break;

            case Constants.SUSHI_MG:
                SceneManager.LoadScene("SushiMG");
                break;

            default:
                Debug.Log(Registry.PreviousMenu + " is not programmed to be possible to return back to from the pause menu.");
                SceneManager.LoadScene("MainMenu");
                break;
        }
    }

    public void OnOptionsButtonClick()
    {
        SceneManager.LoadScene("OptionsMenu");
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
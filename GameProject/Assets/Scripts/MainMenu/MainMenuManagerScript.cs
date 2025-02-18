using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManagerScript : MonoBehaviour
{
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
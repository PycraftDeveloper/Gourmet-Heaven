using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionMenuManagerScript : MonoBehaviour
{
    public void OnLevelOneButtonClick()
    {
        // change things specific to level here :)
        Registry.InGameLevel = true;
        SceneManager.LoadScene("Kitchen");
    }

    public void OnLevelTwoButtonClick()
    {
        // change things specific to level here :)
        Registry.InGameLevel = true;
        SceneManager.LoadScene("Kitchen");
    }

    public void OnBackButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
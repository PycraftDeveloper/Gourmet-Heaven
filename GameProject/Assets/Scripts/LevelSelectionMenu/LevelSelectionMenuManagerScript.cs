using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionMenuManagerScript : MonoBehaviour
{
    public void OnLevelOneButtonClick()
    {
        // change things specific to level here :)
        SceneManager.LoadScene("Kitchen");
    }

    public void OnLevelTwoButtonClick()
    {
        // change things specific to level here :)
        SceneManager.LoadScene("Kitchen");
    }
}
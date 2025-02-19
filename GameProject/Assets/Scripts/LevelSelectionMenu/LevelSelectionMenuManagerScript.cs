using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionMenuManagerScript : MonoBehaviour
{
    public void OnLevelOneButtonClick()
    {
        SceneManager.LoadScene("LevelOne");
    }

    public void OnLevelTwoButtonClick()
    {
        SceneManager.LoadScene("LevelTwo");
    }
}
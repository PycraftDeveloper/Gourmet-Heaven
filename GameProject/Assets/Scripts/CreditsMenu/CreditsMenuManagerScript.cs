using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsMenuManagerScript : MonoBehaviour
{
    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
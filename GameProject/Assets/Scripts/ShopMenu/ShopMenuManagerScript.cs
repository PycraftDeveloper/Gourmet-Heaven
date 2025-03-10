using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopMenuManagerScript : MonoBehaviour
{
    public void OnBackButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
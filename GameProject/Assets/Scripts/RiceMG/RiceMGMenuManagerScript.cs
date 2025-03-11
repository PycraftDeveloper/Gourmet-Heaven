using UnityEngine;
using UnityEngine.SceneManagement;

public class RiceMGMenuManagerScript : MonoBehaviour
{
    public void OnPauseButtonClick()
    {
        SceneManager.LoadScene("PauseMenu");
    }
}
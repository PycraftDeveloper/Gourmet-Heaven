using UnityEngine;
using UnityEngine.SceneManagement;

public class RiceMGMenuManagerScript : MonoBehaviour
{
    public void OnPauseButtonClick()
    {
        Registry.PreviousMenu = Constants.RICE_MG;
        SceneManager.LoadScene("PauseMenu");
    }
}
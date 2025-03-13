using UnityEngine;
using UnityEngine.SceneManagement;

public class TEMP_MG_buttons : MonoBehaviour
{
    public void OnRiceMiniGameButtonClick()
    {
        SceneManager.LoadScene("RiceMG");
    }

    public void OnPauseButtonClick()
    {
        Registry.PreviousMenu = Constants.IN_GAME;
        SceneManager.LoadScene("PauseMenu");
    }
}
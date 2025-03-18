using UnityEngine;

public class TEMP_MG_buttons : MonoBehaviour
{
    public void OnRiceMiniGameButtonClick()
    {
        Registry.GameManagerObject.ChangeScene(Constants.RICE_MG);
    }

    public void OnPauseButtonClick()
    {
        Registry.GameManagerObject.ChangeScene(Constants.PAUSE_MENU);
    }
}
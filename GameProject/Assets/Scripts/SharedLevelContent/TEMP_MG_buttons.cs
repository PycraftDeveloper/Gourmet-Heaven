using UnityEngine;

public class TEMP_MG_buttons : MonoBehaviour
{
    public void OnPauseButtonClick()
    {
        Registry.GameManagerObject.ChangeScene(Constants.PAUSE_MENU);
    }
}
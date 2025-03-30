// The following program was written by Emmie Heane.

using UnityEngine;

public class RiceMGMenuManagerScript : MonoBehaviour
{
    public void OnPauseButtonClick()
    {
        Registry.GameManagerObject.ChangeScene(Constants.PAUSE_MENU);
    }
}
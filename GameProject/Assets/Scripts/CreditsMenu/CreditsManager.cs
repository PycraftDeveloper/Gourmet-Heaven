using UnityEngine;

public class CreditsMenuManagerScript : MonoBehaviour
{
    public void OnBackButtonClicked()
    {
        Registry.GameManagerObject.ChangeScene();
    }
}
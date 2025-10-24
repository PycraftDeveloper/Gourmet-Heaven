using UnityEngine;

public class CreditsMenuManagerScript : MonoBehaviour
{
    public void OnBackButtonClicked()
    {
        Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.ButtonClickSound);
        Registry.GameManagerObject.ChangeScene();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackButtonClicked();
        }
    }
}
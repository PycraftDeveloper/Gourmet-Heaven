using UnityEngine;
using UnityEngine.UI;

public class CreditsMenuManagerScript : MonoBehaviour
{
    public Canvas MenuCanvas;

    public void Start()
    {
        MenuCanvas.worldCamera = Camera.main;
    }

    public void OnBackButtonClicked()
    {
        Registry.CoreGameInfrastructureObject.SFXSource.PlayOneShot(Registry.CoreGameInfrastructureObject.ButtonClickSound);
        Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.PREVIOUS_MENU);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackButtonClicked();
        }
    }
}
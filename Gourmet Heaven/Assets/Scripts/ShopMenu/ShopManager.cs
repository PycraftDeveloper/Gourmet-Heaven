using UnityEngine;

public class ShopMenuManagerScript : MonoBehaviour
{
    public Canvas MenuCanvas;

    public void Start()
    {
        MenuCanvas.worldCamera = Camera.main;
    }

    public void OnBackButtonClick()
    {
        Registry.CoreGameInfrastructureObject.SFXSource.PlayOneShot(Registry.CoreGameInfrastructureObject.ButtonClickSound);
        Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.PREVIOUS_MENU);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Use the escape key as an alternative to the back button.
        {
            Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.PREVIOUS_MENU);
        }
    }
}
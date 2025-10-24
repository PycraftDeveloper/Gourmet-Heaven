using UnityEngine;

public class ShopMenuManagerScript : MonoBehaviour
{
    public void OnBackButtonClick()
    {
        Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.ButtonClickSound);
        Registry.GameManagerObject.ChangeScene();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Use the escape key as an alternative to the back button.
        {
            Registry.GameManagerObject.ChangeScene();
        }
    }
}
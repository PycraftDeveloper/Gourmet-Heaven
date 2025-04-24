using UnityEngine;

public class ShopMenuManagerScript : MonoBehaviour
{
    public void OnBackButtonClick()
    {
        Registry.GameManagerObject.ChangeScene();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackButtonClick();
        }
    }
}
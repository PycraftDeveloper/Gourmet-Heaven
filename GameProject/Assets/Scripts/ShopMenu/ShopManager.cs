using UnityEngine;

public class ShopMenuManagerScript : MonoBehaviour
{
    public void OnBackButtonClick()
    {
        Registry.GameManagerObject.ChangeScene();
    }
}
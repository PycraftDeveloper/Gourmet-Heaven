using UnityEngine;

public class ShopMenuManagerScript : MonoBehaviour
{
    [Header("UI")]
    public Canvas MenuCanvas;

    public void Start()
    {
        MenuCanvas.worldCamera = Camera.main;
    }

    public void OnBackButtonClick()
    {
        Registry.CGI.PlayClickSound();
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Use the escape key as an alternative to the back button.
        {
            Destroy(gameObject);
        }
    }
}
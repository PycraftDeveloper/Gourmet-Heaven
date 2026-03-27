using UnityEngine;

public class ShopMenuManagerScript : MonoBehaviour
{
    [Header("UI")]
    public Canvas MenuCanvas;

    [Header("Navigable Menus")]
    public GameObject MainMenuPrefab;

    public void Start()
    {
        MenuCanvas.worldCamera = Camera.main;
    }

    public void OnBackButtonClick()
    {
        Registry.CGI.SFXSource.PlayOneShot(Registry.CGI.ButtonClickSound);
        Instantiate(MainMenuPrefab);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Use the escape key as an alternative to the back button.
        {
            Instantiate(MainMenuPrefab);
            Destroy(gameObject);
        }
    }
}
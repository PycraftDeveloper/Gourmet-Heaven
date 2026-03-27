using UnityEngine;
using UnityEngine.UI;

public class CreditsMenuManagerScript : MonoBehaviour
{
    [Header("UI")]
    public Canvas MenuCanvas;

    [Header("Navigable Menus")]
    public GameObject MainMenuPrefab;

    public void Start()
    {
        MenuCanvas.worldCamera = Camera.main;
    }

    public void OnBackButtonClicked()
    {
        Registry.CGI.SFXSource.PlayOneShot(Registry.CGI.ButtonClickSound);
        Instantiate(MainMenuPrefab);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackButtonClicked();
        }
    }
}
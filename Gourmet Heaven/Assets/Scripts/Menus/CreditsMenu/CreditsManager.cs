using UnityEngine;
using UnityEngine.UI;

public class CreditsMenuManagerScript : MonoBehaviour
{
    [Header("UI")]
    public Canvas MenuCanvas;

    public void Start()
    {
        MenuCanvas.worldCamera = Camera.main;
    }

    public void OnBackButtonClicked()
    {
        Registry.CGI.PlayClickSound();
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(gameObject);
        }
    }
}
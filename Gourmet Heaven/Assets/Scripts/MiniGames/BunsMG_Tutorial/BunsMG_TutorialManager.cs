using UnityEngine;

public class BunsMG_TutorialManager : MonoBehaviour
{
    [Header("UI")]
    public Canvas MenuCanvas;

    [Header("Navigable Menus")]
    public GameObject Buns_MG_Menu;

    public void Start()
    {
        MenuCanvas.worldCamera = Camera.main;
        MenuCanvas.sortingLayerName = "UI";
    }

    public void OnContinueButtonClicked()
    {
        Instantiate(Buns_MG_Menu);
        Destroy(this.gameObject);
    }
}
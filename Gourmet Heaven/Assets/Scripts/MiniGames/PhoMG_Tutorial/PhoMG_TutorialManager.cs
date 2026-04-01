using UnityEngine;

public class PhoMG_TutorialManager : MonoBehaviour
{
    [Header("UI")]
    public Canvas MenuCanvas;

    [Header("Navigable Menus")]
    public GameObject Pho_MG_Menu;

    public void Start()
    {
        MenuCanvas.worldCamera = Camera.main;
        MenuCanvas.sortingLayerName = "UI";
    }

    public void OnContinueButtonClicked()
    {
        Instantiate(Pho_MG_Menu);
        Destroy(this.gameObject);
    }
}
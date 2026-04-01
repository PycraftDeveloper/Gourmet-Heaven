using UnityEngine;

public class RiceMG_TutorialManager : MonoBehaviour
{
    [Header("UI")]
    public Canvas MenuCanvas;

    [Header("Navigable Menus")]
    public GameObject Rice_MG_Menu;

    public void Start()
    {
        MenuCanvas.worldCamera = Camera.main;
        MenuCanvas.sortingLayerName = "UI";
    }

    public void OnContinueButtonClicked()
    {
        Instantiate(Rice_MG_Menu);
        Destroy(this.gameObject);
    }
}
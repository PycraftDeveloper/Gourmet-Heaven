using UnityEngine;

public class SushiMG_TutorialManager : MonoBehaviour
{
    [Header("UI")]
    public Canvas MenuCanvas;

    [Header("Navigable Menus")]
    public GameObject Sushi_MG_Menu;

    public void Start()
    {
        MenuCanvas.worldCamera = Camera.main;
        MenuCanvas.sortingLayerName = "UI";
    }

    public void OnContinueButtonClicked()
    {
        Instantiate(Sushi_MG_Menu);
        Destroy(this.gameObject);
    }
}
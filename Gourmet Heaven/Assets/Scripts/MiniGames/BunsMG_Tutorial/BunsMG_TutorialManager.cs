using UnityEngine;

public class BunsMG_TutorialManager : MonoBehaviour
{
    public Canvas MenuCanvas;

    public void Start()
    {
        MenuCanvas.worldCamera = Camera.main;
        MenuCanvas.sortingLayerName = "UI";
    }

    public void OnContinueButtonClicked()
    {
        Registry.CGI.ChangeMenu(Constants.BUNS_MG_MENU);
    }
}
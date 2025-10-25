using UnityEngine;

public class PhoMG_TutorialManager : MonoBehaviour
{
    public Canvas MenuCanvas;

    public void Start()
    {
        MenuCanvas.worldCamera = Camera.main;
        MenuCanvas.sortingLayerName = "UI";
    }

    public void OnContinueButtonClicked()
    {
        Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.PHO_MG_MENU);
    }
}
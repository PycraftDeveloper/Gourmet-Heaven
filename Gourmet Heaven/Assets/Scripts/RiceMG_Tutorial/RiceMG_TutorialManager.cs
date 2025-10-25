using UnityEngine;

public class RiceMG_TutorialManager : MonoBehaviour
{
    public Canvas MenuCanvas;

    public void Start()
    {
        MenuCanvas.worldCamera = Camera.main;
        MenuCanvas.sortingLayerName = "UI";
    }

    public void OnContinueButtonClicked()
    {
        Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.RICE_MG_MENU);
    }
}
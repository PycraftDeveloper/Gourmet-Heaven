using UnityEngine;

public class SushiMG_TutorialManager : MonoBehaviour
{
    public Canvas MenuCanvas;

    public void Start()
    {
        MenuCanvas.worldCamera = Camera.main;
        MenuCanvas.sortingLayerName = "UI";
    }

    public void OnContinueButtonClicked()
    {
        Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.SUSHI_MG_MENU);
    }
}
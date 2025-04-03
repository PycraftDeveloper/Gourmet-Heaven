using UnityEngine;

public class LevelSelectionMenuManagerScript : MonoBehaviour
{
    public void OnLevelOneButtonClick()
    {
        Registry.LevelCustomerObject.SetupLevelOne();
        Registry.GameManagerObject.ChangeScene(Constants.KITCHEN);
    }

    public void OnLevelTwoButtonClick()
    {
        Registry.LevelCustomerObject.SetupLevelTwo();
        Registry.GameManagerObject.ChangeScene(Constants.KITCHEN);
    }

    public void OnBackButtonClick()
    {
        Registry.GameManagerObject.ChangeScene();
    }
}
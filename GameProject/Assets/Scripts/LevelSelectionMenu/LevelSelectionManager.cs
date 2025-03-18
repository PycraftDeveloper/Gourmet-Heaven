using UnityEngine;

public class LevelSelectionMenuManagerScript : MonoBehaviour
{
    public void OnLevelOneButtonClick()
    {
        // change things specific to level here :)
        Registry.GameManagerObject.ChangeScene(Constants.KITCHEN);
    }

    public void OnLevelTwoButtonClick()
    {
        // change things specific to level here :)
        Registry.GameManagerObject.ChangeScene(Constants.KITCHEN);
    }

    public void OnBackButtonClick()
    {
        Registry.GameManagerObject.ChangeScene();
    }
}
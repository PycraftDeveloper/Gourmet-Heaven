using UnityEngine;

public class KitchenManager : MonoBehaviour
{
    public void OnCachierPopUpButtonClick()
    {
        Registry.LevelManagerObject.HandleOrderCollection();
    }

    public void OnChoppingBoardPopUpButtonClick()
    {
        Registry.GameManagerObject.ChangeScene(Constants.RICE_MG);
    }

    public void OnCookingPotPopUpButtonClick()
    {
        Registry.GameManagerObject.ChangeScene(Constants.BUNS_MG);
    }

    public void OnPhoBowlPopUpButtonClick()
    {
        Registry.GameManagerObject.ChangeScene(Constants.PHO_MG);
    }

    public void OnRollingMatPopUpButtonClick()
    {
        Registry.GameManagerObject.ChangeScene(Constants.SUSHI_MG);
    }
}
using UnityEngine;

public class KitchenManager : MonoBehaviour
{
    public void OnCachierPopUpButtonClick() // Collect the customer's order when the customer is served at the till point
    {
        Registry.LevelManagerObject.HandleOrderCollection();
    }

    // Allow the pop-ups to act as buttons, transitioning to the associated mini-game
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

    // Allow the player to click on the bin's pop-up to remove the meal from the player's hands.
    public void OnBinPopUpButtonClick()
    {
        Registry.PlayerObject.HoldingMeal = Constants.NOT_HOLDING_MEAL;
    }
}
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
        if (!Registry.RiceMGTutorialShown)
        {
            Registry.RiceMGTutorialShown = true;
            Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.RICE_MG_TUTORIAL_MENU);
        }
        else
        {
            Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.RICE_MG_MENU);
        }
    }

    public void OnCookingPotPopUpButtonClick()
    {
        if (!Registry.BunsMGTutorialShown)
        {
            Registry.BunsMGTutorialShown = true;
            Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.BUNS_MG_TUTORIAL_MENU);
        }
        else
        {
            Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.BUNS_MG_MENU);
        }
    }

    public void OnPhoBowlPopUpButtonClick()
    {
        if (!Registry.PhoMGTutorialShown)
        {
            Registry.PhoMGTutorialShown = true;
            Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.PHO_MG_TUTORIAL_MENU);
        }
        else
        {
            Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.PHO_MG_MENU);
        }
    }

    public void OnRollingMatPopUpButtonClick()
    {
        if (!Registry.SushiMGTutorialShown)
        {
            Registry.SushiMGTutorialShown = true;
            Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.SUSHI_MG_TUTORIAL_MENU);
        }
        else
        {
            Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.SUSHI_MG_MENU);
        }
    }

    // Allow the player to click on the bin's pop-up to remove the meal from the player's hands.
    public void OnBinPopUpButtonClick()
    {
        Registry.PlayerObject.HoldingMeal = Constants.NOT_HOLDING_MEAL;
    }
}
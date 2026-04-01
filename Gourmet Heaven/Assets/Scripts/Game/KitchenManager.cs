using UnityEngine;

public class KitchenManager : MonoBehaviour
{
    [Header("Navigable Menus - Mini Game Tutorials")]
    public GameObject Rice_MG_TutorialMenu;

    public GameObject Buns_MG_TutorialMenu;
    public GameObject Pho_MG_TutorialMenu;
    public GameObject Sushi_MG_TutorialMenu;

    [Header("Navigable Menus - Mini Games")]
    public GameObject Rice_MG_Menu;

    public GameObject Buns_MG_Menu;
    public GameObject Pho_MG_Menu;
    public GameObject Sushi_MG_Menu;

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
            Instantiate(Rice_MG_TutorialMenu);
        }
        else
        {
            Instantiate(Rice_MG_Menu);
        }
    }

    public void OnCookingPotPopUpButtonClick()
    {
        if (!Registry.BunsMGTutorialShown)
        {
            Instantiate(Buns_MG_TutorialMenu);
        }
        else
        {
            Instantiate(Buns_MG_Menu);
        }
    }

    public void OnPhoBowlPopUpButtonClick()
    {
        if (!Registry.PhoMGTutorialShown)
        {
            Instantiate(Pho_MG_TutorialMenu);
        }
        else
        {
            Instantiate(Pho_MG_Menu);
        }
    }

    public void OnRollingMatPopUpButtonClick()
    {
        if (!Registry.SushiMGTutorialShown)
        {
            Instantiate(Sushi_MG_TutorialMenu);
        }
        else
        {
            Instantiate(Sushi_MG_Menu);
        }
    }

    // Allow the player to click on the bin's pop-up to remove the meal from the player's hands.
    public void OnBinPopUpButtonClick()
    {
        Registry.PlayerObject.HoldingMeal = Constants.NOT_HOLDING_MEAL;
    }
}
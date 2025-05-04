using UnityEngine;

public class Pho_MiniGameManager : MonoBehaviour
{
    public GameObject DisplayedPotNoodlesObject;
    public GameObject DisplayedPotBeefObject;
    public GameObject DisplayedBowlCillantroObject;
    public GameObject DisplayedBowlParsleyObject;

    public Sprite[] TargetBowlSprites = new Sprite[5];

    private GameObject CurrentDisplayedIngredient;
    private Pho_Ingredient CurrentIngredient;

    public GameObject TargetBowlObject;
    private Pho_IngredientTarget IngredientTarget;
    private SpriteRenderer IngredientTargetRenderer;

    public GameObject MiniGameFailedPopUp;
    public GameObject MiniGameWinPopUp;

    public GameObject PhoMiniGameTutorial;

    private bool MiniGameLocked = false;

    private int CurrentIngredientIndex = 0;

    private string[] Ingredients = new string[] {
        Constants.PHO_MG_POT_NOODLES,
        Constants.PHO_MG_POT_BEEF,
        Constants.PHO_MG_BOWL_CILLANTRO,
        Constants.PHO_MG_BOWL_PARSLEY };

    private void ReturnToKitchen()
    {
        Registry.GameManagerObject.ChangeScene();
    }

    private void OnMiniGameFailed()
    {
        MiniGameLocked = true;
        MiniGameFailedPopUp.SetActive(true);
        Invoke("ReturnToKitchen", 2f);
    }

    private void ShowMiniGameSucsess()
    {
        MiniGameWinPopUp.SetActive(true);
        Registry.PlayerObject.GetComponent<Player>().HoldingMeal = Constants.PHO;
        Invoke("ReturnToKitchen", 2.0f);
    }

    private void OnMiniGameWin()
    {
        MiniGameLocked = true;

        ShowMiniGameSucsess();
    }

    public void OnContinueButtonClicked()
    {
        PhoMiniGameTutorial.SetActive(false);
        MiniGameLocked = false;
        Registry.NotInTutorialScreenTimeModifier = 1;
    }

    private void Start()
    {
        IngredientTarget = TargetBowlObject.GetComponent<Pho_IngredientTarget>();
        IngredientTargetRenderer = TargetBowlObject.GetComponent<SpriteRenderer>();

        if (!Registry.PhoMGTutorialShown)
        {
            PhoMiniGameTutorial.SetActive(true);
            MiniGameLocked = true;
            Registry.SushiMGTutorialShown = true;
            Registry.NotInTutorialScreenTimeModifier = 0;
        }
    }

    private void Update()
    {
        if (!MiniGameLocked)
        {
            if (CurrentDisplayedIngredient == null)
            {
                if (Ingredients[CurrentIngredientIndex] == Constants.PHO_MG_POT_NOODLES)
                {
                    CurrentDisplayedIngredient = DisplayedPotNoodlesObject;
                }
                else if (Ingredients[CurrentIngredientIndex] == Constants.PHO_MG_POT_BEEF)
                {
                    CurrentDisplayedIngredient = DisplayedPotBeefObject;
                }
                else if (Ingredients[CurrentIngredientIndex] == Constants.PHO_MG_BOWL_CILLANTRO)
                {
                    CurrentDisplayedIngredient = DisplayedBowlCillantroObject;
                }
                else if (Ingredients[CurrentIngredientIndex] == Constants.PHO_MG_BOWL_PARSLEY)
                {
                    CurrentDisplayedIngredient = DisplayedBowlParsleyObject;
                }
                CurrentDisplayedIngredient.SetActive(true);
                CurrentIngredient = CurrentDisplayedIngredient.GetComponent<Pho_Ingredient>();
            }

            if (Input.touchCount > 0) // if touch input is used
            {
                CurrentIngredient.DropIngredient = true;
            }
            else if (Input.GetMouseButton(0)) // if mouse click is used
            {
                CurrentIngredient.DropIngredient = true;
            }

            if (IngredientTarget.IngredientInTargetToggle)
            {
                CurrentDisplayedIngredient.SetActive(false);
                CurrentDisplayedIngredient = null;
                CurrentIngredientIndex++;
                IngredientTarget.IngredientInTargetToggle = false;

                if (CurrentIngredientIndex >= Ingredients.Length)
                {
                    OnMiniGameWin();
                }
                else
                {
                    IngredientTargetRenderer.sprite = TargetBowlSprites[CurrentIngredientIndex];
                }
            }
            else if (CurrentIngredient.MissedTarget)
            {
                OnMiniGameFailed();
            }
        }
    }
}
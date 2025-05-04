using UnityEngine;

public class Sushi_MiniGameManager : MonoBehaviour
{
    private string[] TargetIngredientOrder = new string[5] {
        Constants.SUSHI_MG_SEAWEED,
        Constants.SUSHI_MG_RICE,
        Constants.SUSHI_MG_TUNA,
        Constants.SUSHI_MG_TUNA,
        Constants.SUSHI_MG_WASABI };

    private string[] CurrentIngredientOrder = new string[5];

    public GameObject RiceHitbox;
    public GameObject SeaweedHitbox;
    public GameObject WasabiHitbox;
    public GameObject TunaHitbox;

    private Sushi_IngredientSpawn RiceIngredientSpawn;
    private Sushi_IngredientSpawn SeaweedIngredientSpawn;
    private Sushi_IngredientSpawn WasabiIngredientSpawn;
    private Sushi_IngredientSpawn TunaIngredientSpawn;

    public GameObject DisplayedRiceObject;
    public GameObject DisplayedSeaweedObject;
    public GameObject DisplayedWasabiObject;
    public GameObject DisplayedTunaObject;
    public GameObject DisplayedShinyTunaObject;

    public GameObject MiniGameFailedPopUp;
    public GameObject MiniGameWinPopUp;

    public GameObject SushiMiniGameTutorial;

    public GameObject SushiRollingAnimation;

    public bool IngredientSpawned = false;
    private bool IngredientListChanged = false;

    private int IngredientIndex = 0;

    private bool MiniGameLocked = false;

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
        Registry.PlayerObject.GetComponent<Player>().HoldingMeal = Constants.SUSHI;
        Invoke("ReturnToKitchen", 2.0f);
    }

    private void OnMiniGameWin()
    {
        MiniGameLocked = true;

        SushiRollingAnimation.SetActive(true);
        Animator SushiRollingAnimator = SushiRollingAnimation.GetComponent<Animator>();
        SushiRollingAnimator.Play("SushiRolling");

        DisplayedRiceObject.SetActive(false);
        DisplayedSeaweedObject.SetActive(false);
        DisplayedWasabiObject.SetActive(false);
        DisplayedTunaObject.SetActive(false);
        DisplayedShinyTunaObject.SetActive(false);

        Invoke("ShowMiniGameSucsess", 1.2f); // delay until animation finished  (1.017 seconds)
    }

    public void OnContinueButtonClicked()
    {
        SushiMiniGameTutorial.SetActive(false);
        MiniGameLocked = false;
        Registry.NotInTutorialScreenTimeModifier = 1;
    }

    private void Start()
    {
        RiceIngredientSpawn = RiceHitbox.GetComponent<Sushi_IngredientSpawn>();
        SeaweedIngredientSpawn = SeaweedHitbox.GetComponent<Sushi_IngredientSpawn>();
        WasabiIngredientSpawn = WasabiHitbox.GetComponent<Sushi_IngredientSpawn>();
        TunaIngredientSpawn = TunaHitbox.GetComponent<Sushi_IngredientSpawn>();

        if (!Registry.SushiMGTutorialShown)
        {
            SushiMiniGameTutorial.SetActive(true);
            MiniGameLocked = true;
            Registry.SushiMGTutorialShown = true;
            Registry.NotInTutorialScreenTimeModifier = 0;
        }
    }

    private void Update()
    {
        RiceIngredientSpawn.MiniGameLocked = MiniGameLocked;
        SeaweedIngredientSpawn.MiniGameLocked = MiniGameLocked;
        WasabiIngredientSpawn.MiniGameLocked = MiniGameLocked;
        TunaIngredientSpawn.MiniGameLocked = MiniGameLocked;

        if (!MiniGameLocked)
        {
            if (RiceIngredientSpawn.IngredientDraggedIntoTargetToggle)
            {
                DisplayedRiceObject.SetActive(true);
                RiceIngredientSpawn.IngredientDraggedIntoTargetToggle = false;
                CurrentIngredientOrder[IngredientIndex] = Constants.SUSHI_MG_RICE;
                IngredientListChanged = true;
                IngredientIndex++;
            }

            if (SeaweedIngredientSpawn.IngredientDraggedIntoTargetToggle)
            {
                DisplayedSeaweedObject.SetActive(true);
                SeaweedIngredientSpawn.IngredientDraggedIntoTargetToggle = false;
                CurrentIngredientOrder[IngredientIndex] = Constants.SUSHI_MG_SEAWEED;
                IngredientListChanged = true;
                IngredientIndex++;
            }

            if (WasabiIngredientSpawn.IngredientDraggedIntoTargetToggle)
            {
                DisplayedWasabiObject.SetActive(true);
                WasabiIngredientSpawn.IngredientDraggedIntoTargetToggle = false;
                CurrentIngredientOrder[IngredientIndex] = Constants.SUSHI_MG_WASABI;
                IngredientListChanged = true;
                IngredientIndex++;
            }

            if (TunaIngredientSpawn.IngredientDraggedIntoTargetToggle)
            {
                if (!DisplayedTunaObject.activeSelf)
                {
                    DisplayedTunaObject.SetActive(true);
                }
                else
                {
                    DisplayedShinyTunaObject.SetActive(true);
                }
                TunaIngredientSpawn.IngredientDraggedIntoTargetToggle = false;
                CurrentIngredientOrder[IngredientIndex] = Constants.SUSHI_MG_TUNA;
                IngredientListChanged = true;
                IngredientIndex++;
            }

            if (IngredientListChanged)
            {
                IngredientListChanged = false;
                for (int i = 0; i < IngredientIndex; i++)
                {
                    if (CurrentIngredientOrder[i] != TargetIngredientOrder[i])
                    {
                        OnMiniGameFailed();
                        break;
                    }
                }

                if (IngredientIndex == 5 && !MiniGameLocked)
                {
                    OnMiniGameWin();
                }
            }
        }
    }
}
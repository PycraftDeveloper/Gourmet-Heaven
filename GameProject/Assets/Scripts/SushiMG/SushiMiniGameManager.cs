using UnityEngine;

public class SushiMiniGameManager : MonoBehaviour
{
    private string[] TargetIngredientOrder = new string[5] { Constants.SEAWEED, Constants.RICE, Constants.TUNA, Constants.TUNA, Constants.WASABI };
    private string[] CurrentIngredientOrder = new string[5];

    public GameObject RiceHitbox;
    public GameObject SeaweedHitbox;
    public GameObject WasabiHitbox;
    public GameObject TunaHitbox;

    private IngredientSpawn RiceIngredientSpawn;
    private IngredientSpawn SeaweedIngredientSpawn;
    private IngredientSpawn WasabiIngredientSpawn;
    private IngredientSpawn TunaIngredientSpawn;

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
        RiceIngredientSpawn = RiceHitbox.GetComponent<IngredientSpawn>();
        SeaweedIngredientSpawn = SeaweedHitbox.GetComponent<IngredientSpawn>();
        WasabiIngredientSpawn = WasabiHitbox.GetComponent<IngredientSpawn>();
        TunaIngredientSpawn = TunaHitbox.GetComponent<IngredientSpawn>();

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
                CurrentIngredientOrder[IngredientIndex] = Constants.RICE;
                IngredientListChanged = true;
                IngredientIndex++;
            }

            if (SeaweedIngredientSpawn.IngredientDraggedIntoTargetToggle)
            {
                DisplayedSeaweedObject.SetActive(true);
                SeaweedIngredientSpawn.IngredientDraggedIntoTargetToggle = false;
                CurrentIngredientOrder[IngredientIndex] = Constants.SEAWEED;
                IngredientListChanged = true;
                IngredientIndex++;
            }

            if (WasabiIngredientSpawn.IngredientDraggedIntoTargetToggle)
            {
                DisplayedWasabiObject.SetActive(true);
                WasabiIngredientSpawn.IngredientDraggedIntoTargetToggle = false;
                CurrentIngredientOrder[IngredientIndex] = Constants.WASABI;
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
                CurrentIngredientOrder[IngredientIndex] = Constants.TUNA;
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
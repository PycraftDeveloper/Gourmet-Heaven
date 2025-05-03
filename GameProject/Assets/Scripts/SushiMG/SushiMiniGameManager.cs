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

    private void OnMiniGameWin()
    {
        MiniGameLocked = true;
        MiniGameWinPopUp.SetActive(true);
        Registry.PlayerObject.GetComponent<Player>().HoldingMeal = Constants.SUSHI;
        Invoke("ReturnToKitchen", 2f);
    }

    private void Start()
    {
        RiceIngredientSpawn = RiceHitbox.GetComponent<IngredientSpawn>();
        SeaweedIngredientSpawn = SeaweedHitbox.GetComponent<IngredientSpawn>();
        WasabiIngredientSpawn = WasabiHitbox.GetComponent<IngredientSpawn>();
        TunaIngredientSpawn = TunaHitbox.GetComponent<IngredientSpawn>();
    }

    private void Update()
    {
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

                if (IngredientIndex == 5)
                {
                    OnMiniGameWin();
                }
            }
        }
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pho_MiniGameManager : MonoBehaviour
{
    // Store all the prefabs for the ingredients that fall from containers
    public GameObject NoodleIngredientPrefab;

    public GameObject BeefIngredientPrefab;
    public GameObject CillantroIngredientPrefab;
    public GameObject ParsleyIngredientPrefab;

    // Store all the containers that hold the ingredients
    public GameObject NoodleContainerObject;

    public GameObject BeefContainerObject;
    public GameObject CillantroContainerObject;
    public GameObject ParsleyContainerObject;

    // Store all the different 'in preparation' styles for the pho bowl to switch to as ingredients are added successfully.
    public Sprite[] TargetBowlSprites = new Sprite[5];

    // Store the different varieties for the empty tipped bowl and pan.
    public Sprite[] TippedBowlSprites = new Sprite[2];

    private GameObject CurrentIngredientObject; // Stores the currently instantiated ingredient prefab.
    private Pho_Ingredient CurrentIngredient; // Stores the associated script for the current ingredient instance.

    private GameObject CurrentContainerObject; // Store the currently displayed container object.
    private Image CurrentContainerImage;

    public GameObject TargetBowlObject; // Store the pho ingredient target bowl object.
    private Pho_IngredientTarget IngredientTarget;
    private Image IngredientTargetImage;

    public GameObject MiniGameFailedPopUp;
    public GameObject MiniGameWinPopUp;
    public Canvas BackgroundMenuCanvas;
    public Canvas ForegroundMenuCanvas;

    public AudioClip BackgroundMusic;

    public TextMeshProUGUI TimerText;

    private bool MiniGameLocked = false;
    private bool UserInput = true;

    private int CurrentIngredientIndex = 0; // Used to determine which ingredient to show next. Note there is no need to worry about order in this mini-game.

    private float MiniGameTimer = Constants.PHO_MG_MINI_GAME_TIME;

    private string[] Ingredients = new string[] { // Store the ingredients needed and the order thhey are to appear.
        Constants.PHO_MG_POT_NOODLES,
        Constants.PHO_MG_POT_BEEF,
        Constants.PHO_MG_BOWL_CILLANTRO,
        Constants.PHO_MG_BOWL_PARSLEY };

    private void ReturnToKitchen()
    {
        Registry.InMiniGame = false;
        Registry.CoreGameInfrastructureObject.CloseMenu();
    }

    private void OnMiniGameFailed()
    {
        MiniGameLocked = true;
        MiniGameFailedPopUp.SetActive(true);
        Registry.CoreGameInfrastructureObject.GameMusicSource.UnPause();
        Registry.CoreGameInfrastructureObject.musicSource.Stop();
        Invoke("ReturnToKitchen", 2f);
    }

    private void ShowMiniGameSucsess()
    {
        MiniGameWinPopUp.SetActive(true);
        Registry.CoreGameInfrastructureObject.GameMusicSource.UnPause();
        Registry.CoreGameInfrastructureObject.musicSource.Stop();
        Registry.PlayerObject.GetComponent<Player>().HoldingMeal = Constants.PHO;
        Invoke("ReturnToKitchen", 2.0f);
    }

    private void OnMiniGameWin() // Unlike the sushi mini-game there are no animations to be played on completion of this mini-game, so can continue to displaying splash screen.
    {
        MiniGameLocked = true;

        Registry.PhoMGTutorialShown = true; // Prevent the tutorial screen for the mini-game from showing once the player completes a mini-game perfectly.

        ShowMiniGameSucsess();
    }

    private void Start()
    {
        if (Registry.CoreGameInfrastructureObject.musicSource.clip != BackgroundMusic)
        {
            Registry.CoreGameInfrastructureObject.musicSource.clip = BackgroundMusic;
            Registry.CoreGameInfrastructureObject.musicSource.loop = false;
        }

        Registry.CoreGameInfrastructureObject.musicSource.Play();

        Registry.CoreGameInfrastructureObject.GameMusicSource.Pause();
        Registry.InMiniGame = true;

        BackgroundMenuCanvas.worldCamera = Camera.main;
        BackgroundMenuCanvas.sortingLayerName = "UI";

        ForegroundMenuCanvas.worldCamera = Camera.main;
        ForegroundMenuCanvas.sortingLayerName = "UI";

        IngredientTarget = TargetBowlObject.GetComponent<Pho_IngredientTarget>();
        IngredientTargetImage = TargetBowlObject.GetComponent<Image>();
    }

    private void SetupIngredientSource()
    {
        // Set the bowl to be the correct ingredient type.
        if (Ingredients[CurrentIngredientIndex] == Constants.PHO_MG_POT_NOODLES)
        {
            CurrentContainerObject = NoodleContainerObject;
        }
        else if (Ingredients[CurrentIngredientIndex] == Constants.PHO_MG_POT_BEEF)
        {
            CurrentContainerObject = BeefContainerObject;
        }
        else if (Ingredients[CurrentIngredientIndex] == Constants.PHO_MG_BOWL_CILLANTRO)
        {
            CurrentContainerObject = CillantroContainerObject;
        }
        else if (Ingredients[CurrentIngredientIndex] == Constants.PHO_MG_BOWL_PARSLEY)
        {
            CurrentContainerObject = ParsleyContainerObject;
        }
        CurrentContainerImage = CurrentContainerObject.GetComponent<Image>();

        CurrentContainerObject.SetActive(true);
    }

    private void SetupIngredientDrop()
    {
        // Set the dropped ingredient to be the correct type.
        if (Ingredients[CurrentIngredientIndex] == Constants.PHO_MG_POT_NOODLES)
        {
            CurrentIngredientObject = Instantiate(NoodleIngredientPrefab);
        }
        else if (Ingredients[CurrentIngredientIndex] == Constants.PHO_MG_POT_BEEF)
        {
            CurrentIngredientObject = Instantiate(BeefIngredientPrefab);
        }
        else if (Ingredients[CurrentIngredientIndex] == Constants.PHO_MG_BOWL_CILLANTRO)
        {
            CurrentIngredientObject = Instantiate(CillantroIngredientPrefab);
        }
        else if (Ingredients[CurrentIngredientIndex] == Constants.PHO_MG_BOWL_PARSLEY)
        {
            CurrentIngredientObject = Instantiate(ParsleyIngredientPrefab);
        }
        CurrentIngredient = CurrentIngredientObject.GetComponent<Pho_Ingredient>();
        Rigidbody2D CurrentDisplayedIngredientObjectRigidBody = CurrentContainerObject.GetComponent<Rigidbody2D>();
        CurrentIngredient.transform.position = CurrentDisplayedIngredientObjectRigidBody.position; // Spawn the ingredient where the bowl was as the player tapped.

        CurrentContainerImage.sprite = TippedBowlSprites[1]; // change the bowl sprite to be the tipped version.
        if (CurrentIngredientIndex < 2)
        {
            CurrentContainerImage.sprite = TippedBowlSprites[0];
        }
    }

    private void Update()
    {
        if (!MiniGameLocked)
        {
            // Update the timer text for the mini-game.
            if (MiniGameTimer < 10)
            {
                TimerText.text = "00:0" + (int)MiniGameTimer;
            }
            else
            {
                TimerText.text = "00:" + (int)MiniGameTimer;
            }
            MiniGameTimer -= Time.deltaTime;

            if (CurrentContainerObject == null)
            {
                SetupIngredientSource(); // Set-up a new ingredient source when there is none displayed currently on-screen.
            }

            // Determine if the conditions are correct for the player to drop an ingredient.
            if (CurrentIngredientObject == null)
            {
                if (Input.touchCount > 0) // if touch input is used
                {
                    if (!UserInput)
                    {
                        UserInput = true;
                        SetupIngredientDrop();
                    }
                }
                else if (Input.GetMouseButton(0)) // if mouse click is used
                {
                    if (!UserInput)
                    {
                        UserInput = true;
                        SetupIngredientDrop();
                    }
                }
                else
                {
                    UserInput = false; // Used to prevent the player from pressing and holding to drop multiple ingredients.
                }
            }

            if (IngredientTarget.IngredientInTargetToggle) // Determine if the ingredient reached the target - note here that the type of ingredient doesn't matter.
            {
                // Destroy the ingredient, reset the toggle and prepare the game for the next ingredient.
                Destroy(CurrentIngredientObject);
                CurrentIngredientObject = null;
                CurrentIngredientIndex++;
                IngredientTarget.IngredientInTargetToggle = false;
                CurrentContainerObject.SetActive(false);
                CurrentContainerObject = null;

                // This code was worked on by Joshua Cossar (v)
                int IngredientSound = Random.Range(0, 2);
                if (IngredientSound == 0)
                {
                    Registry.CoreGameInfrastructureObject.SFXSource.PlayOneShot(Registry.CoreGameInfrastructureObject.SoupSplash1);
                }
                if (IngredientSound == 1)
                {
                    Registry.CoreGameInfrastructureObject.SFXSource.PlayOneShot(Registry.CoreGameInfrastructureObject.SoupSplash2);
                }
                // This code was worked on by Joshua Cossar (^)

                // Determine if there are no more ingredients left, in which case the player has won the mini-game
                if (CurrentIngredientIndex >= Ingredients.Length)
                {
                    OnMiniGameWin();
                }
                else
                {
                    IngredientTargetImage.sprite = TargetBowlSprites[CurrentIngredientIndex]; // otherwise, update the bowl sprite to show the next ingredient.
                }
            }
            else if ((CurrentIngredientObject != null && CurrentIngredient.MissedTarget) || MiniGameTimer <= 0) // If the ingredient misses the bowl, or timer runs out...
            {
                OnMiniGameFailed(); // mini game failed.
            }
        }
    }
}
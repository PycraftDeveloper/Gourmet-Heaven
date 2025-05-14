using TMPro;
using UnityEngine;

public class Sushi_MiniGameManager : MonoBehaviour // Used to manage the starting, ending and order for the sushi mini-game.
{
    // Determine the order the ingredients need to be in for a successful sushi roll.
    private string[] TargetIngredientOrder = new string[5] {
        Constants.SUSHI_MG_SEAWEED,
        Constants.SUSHI_MG_RICE,
        Constants.SUSHI_MG_TUNA,
        Constants.SUSHI_MG_TUNA,
        Constants.SUSHI_MG_WASABI };

    // The order the player has placed the ingredients in.
    private string[] CurrentIngredientOrder = new string[5];

    // Store the ingredient spawning locations
    public GameObject RiceHitbox;

    public GameObject SeaweedHitbox;
    public GameObject WasabiHitbox;
    public GameObject TunaHitbox;

    // Get the ingredient spawning locations scripts
    private Sushi_IngredientSpawn RiceIngredientSpawn;

    private Sushi_IngredientSpawn SeaweedIngredientSpawn;
    private Sushi_IngredientSpawn WasabiIngredientSpawn;
    private Sushi_IngredientSpawn TunaIngredientSpawn;

    // Store the different stages of the sushi rolling mat (to indicate progress)
    public GameObject DisplayedRiceObject;

    public GameObject DisplayedSeaweedObject;
    public GameObject DisplayedWasabiObject;
    public GameObject DisplayedTunaObject;
    public GameObject DisplayedShinyTunaObject;

    // Store the pop-ups for the mini-game to show at the end whether the player won or lost.
    public GameObject MiniGameFailedPopUp;

    public GameObject MiniGameWinPopUp;

    // Store the tutorial pop-up for the mini-game
    public GameObject SushiMiniGameTutorial;

    public GameObject SushiRollingAnimation; // The sushi rolling animation object, used to play the animation when the player wins the mini-game.

    public TextMeshProUGUI TimerText; // The timer text object, used to display the time remaining in the mini-game. (Once zero, the mini-game is failed)

    public bool IngredientSpawned = false; // Keep track of if an ingredient has been spawned, to prevent other ingredients from also being spawned.
    private bool IngredientListChanged = false; // Used to only compare the ingredient list when it has changed, to prevent unnecessary checks.

    private int IngredientIndex = 0; // Used to keep track of the current index in the ingredient list, effectively the next ingredient to be added.

    private bool MiniGameLocked = false; // Used to prevent the player from interacting with the mini-game when either the tutorial or splash art is shown

    private float MiniGameTimer = Constants.SUSHI_MG_MINI_GAME_TIME; // how many seconds the mini-game has left to run for.

    private void ReturnToKitchen()
    {
        Registry.GameManagerObject.ChangeScene(); // used to go back to the previous scene (the kitchen)
    }

    private void OnMiniGameFailed() // Called immediately after the mini-game has failed to show the splash art.
    {
        MiniGameLocked = true; // prevent any additional interactions with the mini-game
        MiniGameFailedPopUp.SetActive(true); // show failed splash art
        Invoke("ReturnToKitchen", Constants.MINI_GAME_SPLASH_ART_DURATION); // return to the kitchen after 2 seconds (allows time for the player to see the failed splash art)
    }

    private void ShowMiniGameSucsess() // Called immediately after the mini-game has been won to show the splash art.
    {
        MiniGameWinPopUp.SetActive(true); // show success splash art
        Registry.PlayerObject.GetComponent<Player>().HoldingMeal = Constants.SUSHI; // set the player's current meal to sushi
        Invoke("ReturnToKitchen", Constants.MINI_GAME_SPLASH_ART_DURATION); // return to the kitchen after 2 seconds (allows time for the player to see the success splash art)
    }

    private void OnMiniGameWin() // Called immediately after the mini-game has been won to allow the animation to play and to lock the scene before progressing to the splash art
    {
        MiniGameLocked = true;

        Registry.SushiMGTutorialShown = true; // Only hide the tutorial once the player has completed the game successfully.

        SushiRollingAnimation.SetActive(true); // Show the rolling animation
        Animator SushiRollingAnimator = SushiRollingAnimation.GetComponent<Animator>();
        SushiRollingAnimator.Play("SushiRolling");

        // Immediately hide the individual ingredients how the animation is on top and playing.
        DisplayedRiceObject.SetActive(false);
        DisplayedSeaweedObject.SetActive(false);
        DisplayedWasabiObject.SetActive(false);
        DisplayedTunaObject.SetActive(false);
        DisplayedShinyTunaObject.SetActive(false);

        Invoke("ShowMiniGameSucsess", 1.2f); // delay until animation finished  (1.017 seconds)
    }

    public void OnContinueButtonClicked() // Used in the tutorial to allow the player to continue to the mini-game.
    {
        SushiMiniGameTutorial.SetActive(false); // Hide the tutorial
        MiniGameLocked = false; // Unlock the mini-game
        Registry.NotInTutorialScreenTimeModifier = 1; // Allow the game to continue running as normal (as the time in the game pauses whilst in the tutorial screen).
    }

    private void Start()
    {
        RiceIngredientSpawn = RiceHitbox.GetComponent<Sushi_IngredientSpawn>();
        SeaweedIngredientSpawn = SeaweedHitbox.GetComponent<Sushi_IngredientSpawn>();
        WasabiIngredientSpawn = WasabiHitbox.GetComponent<Sushi_IngredientSpawn>();
        TunaIngredientSpawn = TunaHitbox.GetComponent<Sushi_IngredientSpawn>();

        if (!Registry.SushiMGTutorialShown) // Display the tutorial
        {
            SushiMiniGameTutorial.SetActive(true);
            MiniGameLocked = true; // Lock the mini game until the player has read the tutorial
            Registry.NotInTutorialScreenTimeModifier = 0; // Stop the game from running whilst the player is reading the tutorial
        }
    }

    private void Update()
    {
        // Make sure if the game is locked, this is passed through to the ingredient spawns
        RiceIngredientSpawn.MiniGameLocked = MiniGameLocked;
        SeaweedIngredientSpawn.MiniGameLocked = MiniGameLocked;
        WasabiIngredientSpawn.MiniGameLocked = MiniGameLocked;
        TunaIngredientSpawn.MiniGameLocked = MiniGameLocked;

        if (!MiniGameLocked) // If the mini-game is playing
        {
            // Used to ensure the timer looks correct regardless of the number of numbers in the timer.
            if (MiniGameTimer < 10)
            {
                TimerText.text = "00:0" + (int)MiniGameTimer;
            }
            else
            {
                TimerText.text = "00:" + (int)MiniGameTimer;
            }
            MiniGameTimer -= Time.deltaTime;

            if (RiceIngredientSpawn.IngredientDraggedIntoTargetToggle) // check if the player has dragged rice into the target area
            {
                DisplayedRiceObject.SetActive(true); // show the rice on the sushi mat
                RiceIngredientSpawn.IngredientDraggedIntoTargetToggle = false; // reset the toggle
                CurrentIngredientOrder[IngredientIndex] = Constants.SUSHI_MG_RICE; // add the rice to the player's ingredient order
                IngredientListChanged = true; // trigger the game to check if this is the correct next ingredient
                IngredientIndex++; // increment the index to the next ingredient
                Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.SushiSound); // Added by Joshua Cossar
            }

            if (SeaweedIngredientSpawn.IngredientDraggedIntoTargetToggle) // check if the player has dragged seaweed into the target area
            {
                DisplayedSeaweedObject.SetActive(true);
                SeaweedIngredientSpawn.IngredientDraggedIntoTargetToggle = false;
                CurrentIngredientOrder[IngredientIndex] = Constants.SUSHI_MG_SEAWEED;
                IngredientListChanged = true;
                IngredientIndex++;
                Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.SushiSound); // Added by Joshua Cossar
            }

            if (WasabiIngredientSpawn.IngredientDraggedIntoTargetToggle) // check if the player has dragged wasabi into the target area
            {
                DisplayedWasabiObject.SetActive(true);
                WasabiIngredientSpawn.IngredientDraggedIntoTargetToggle = false;
                CurrentIngredientOrder[IngredientIndex] = Constants.SUSHI_MG_WASABI;
                IngredientListChanged = true;
                IngredientIndex++;
                Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.SushiSound); // Added by Joshua Cossar
            }

            if (TunaIngredientSpawn.IngredientDraggedIntoTargetToggle) // check if the player has dragged (any) tuna into the target area
            {
                if (!DisplayedTunaObject.activeSelf) // Show the normal tuna first
                {
                    DisplayedTunaObject.SetActive(true);
                }
                else // Then show the shiny variety.
                {
                    DisplayedShinyTunaObject.SetActive(true);
                }
                TunaIngredientSpawn.IngredientDraggedIntoTargetToggle = false;
                CurrentIngredientOrder[IngredientIndex] = Constants.SUSHI_MG_TUNA;
                IngredientListChanged = true;
                IngredientIndex++;
                Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.SushiSound); // Added by Joshua Cossar
            }

            if (IngredientListChanged) // Check if the player's ingredient list has changed.
            {
                IngredientListChanged = false; // Reset the flag
                for (int i = 0; i < IngredientIndex; i++) // Compare each element in the player's ingredient list to the target ingredient list, up to the player's current progress.
                {
                    if (CurrentIngredientOrder[i] != TargetIngredientOrder[i])
                    {
                        OnMiniGameFailed(); // If there is a miss-match, the mini-game is failed.
                        break;
                    }
                }

                if (IngredientIndex == 5 && !MiniGameLocked) // If all the ingredients are present and in the correct order, the mini-game is won.
                {
                    OnMiniGameWin();
                }
            }

            if (MiniGameTimer <= 0) // If the player has ran out of time.
            {
                OnMiniGameFailed();
            }
        }
    }
}
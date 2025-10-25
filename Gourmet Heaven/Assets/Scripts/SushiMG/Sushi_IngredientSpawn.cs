using UnityEngine;

public class Sushi_IngredientSpawn : MonoBehaviour // This script is attached to the areas the player needs to drag the ingredients from
                                                   // and handles the automatic spawning of the ingredients when the player touches the screen.
{
    public GameObject IngredientPrefab;
    private GameObject SpawnedIngredientObject; // The instance of the ingredient prefab that currently exists.
    private Sushi_Ingredient SpawnedIngredient; // The Sushi_Ingredient script attached to the spawned ingredient prefab.

    public Sushi_MiniGameManager SushiMiniGameManager; // the mini-game manager script, used to control the mini-game.

    private bool IngredientSpawned = false; // Used to determine if the SpawnedIngredientObject is null or not.
    public bool IngredientDraggedIntoTargetToggle = false; // Used as a toggle to determine if the ingredient has been dragged into the target area, used\
    // to determine the mini-game's next step.

    public bool MiniGameLocked = false; // Used to prevent the player from interacting with this section of the mini-game.

    private void HandleTouch(Vector2 TouchPosition) // Used to determine where on-screen the player touched, check if the player touched the spawning location
                                                    // and then also determine if a new ingredient is to be spawned.
    {
        Vector2 WorldPosition = Camera.main.ScreenToWorldPoint(TouchPosition);
        Collider2D RaycastHit = Physics2D.OverlapPoint(WorldPosition);

        if (RaycastHit != null && RaycastHit.transform == transform && !SushiMiniGameManager.IngredientSpawned) // Check if player touched the target spawning location.
        {
            SpawnedIngredientObject = Instantiate(IngredientPrefab, WorldPosition, transform.rotation);
            SpawnedIngredientObject.transform.SetParent(SushiMiniGameManager.gameObject.transform);
            SpawnedIngredient = SpawnedIngredientObject.GetComponent<Sushi_Ingredient>();
            SpawnedIngredient.SetPosition(TouchPosition);
            SushiMiniGameManager.IngredientSpawned = true; // Keep track that an ingredient has been spawned, ensuring the player can't use multiple inputs to try and spawn multiple ingredients at a time
            IngredientSpawned = true;
        }
    }

    private void Update()
    {
        if (MiniGameLocked) // check if the mini-game has been locked, in which case prevent any further interactions.
        {
            if (SpawnedIngredient != null) // Reset any spawned ingredients
            {
                SpawnedIngredient = null;
                Destroy(SpawnedIngredientObject);

                if (IngredientSpawned)
                {
                    SushiMiniGameManager.IngredientSpawned = false;
                    IngredientSpawned = false;
                }
            }
            return;
        }
        if (SpawnedIngredient == null) // Check if there is no spawned ingredient yet, and check for events that can spawn it.
        {
            if (Input.touchCount > 0) // if touch input is used
            {
                UnityEngine.Touch touch = Input.GetTouch(0); // get the first finger (only guaranteed input)

                HandleTouch(touch.position); // handle position adjustments, using finger position on-screen.
            }
            else if (Input.GetMouseButton(0)) // if mouse click is used
            {
                HandleTouch(Input.mousePosition); // handle position adjustments, using mouse position.
            }
        }
        else // otherwise, check if the player is still performing that input, in which case move the ingredient around the screen. Otherwise the ingredient should be considered as dropped.
        {
            if (Input.touchCount > 0) // if touch input is used
            {
                UnityEngine.Touch touch = Input.GetTouch(0); // get the first finger (only guaranteed input)

                SpawnedIngredient.SetPosition(touch.position);
            }
            else if (Input.GetMouseButton(0)) // if mouse click is used
            {
                SpawnedIngredient.SetPosition(Input.mousePosition);
            }
            else // Consider ingredient dropped
            {
                if (SpawnedIngredient.InTarget) // if dropped into the right place
                {
                    IngredientDraggedIntoTargetToggle = true; // Used to indicate this success to the game manager
                }

                // garbage collect the spawned ingredient - the game manager will automatically handle updates to the way the rolling mat look.
                SpawnedIngredient = null;
                Destroy(SpawnedIngredientObject);

                if (IngredientSpawned)
                {
                    SushiMiniGameManager.IngredientSpawned = false;
                    IngredientSpawned = false;
                }
            }
        }
    }
}
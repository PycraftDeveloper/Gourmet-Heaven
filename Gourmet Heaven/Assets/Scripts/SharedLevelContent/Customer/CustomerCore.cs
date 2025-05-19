using System.Collections;
using UnityEngine;

// Note: This class is inherited by both varieties of customer, so any reference to 'customer' here refers to both types of customer.

public class CustomerCore : MonoBehaviour
{
    public Animator _Animator;
    public Rigidbody2D _RigidBody2D;
    public Renderer _Renderer;
    public GameObject PatienceMeter;
    protected Animator PatienceMeterAnimator;

    public RuntimeAnimatorController[] AnimationControllers = new RuntimeAnimatorController[8]; // Stores each of the 8 different styles for the animations.

    public string CurrentLocation = Constants.KITCHEN; // By default the current location is in the kitchen.

    public int ModelIndex;
    public int CustomerTablePosition; // Stores the customer's position in the restaurant (if applicable).

    public bool DeSpawn = false; // Used to determine when the customer can be garbage collected.

    public float Patience;
    public float InitialPatience;

    public Vector2 CurrentPosition;

    public Coroutine PatienceCoroutine;

    protected virtual void Awake()
    {
        Initialise();
    }

    public virtual void Initialise()
    {
        DontDestroyOnLoad(gameObject); // Ensure the customer persists across scene changes. Its lifetime is controlled elsewhere.

        _Animator = GetComponent<Animator>();
        _RigidBody2D = GetComponent<Rigidbody2D>();
        _Renderer = GetComponent<Renderer>();

        if (PatienceMeter != null)
        {
            PatienceMeterAnimator = PatienceMeter.GetComponent<Animator>();
        }

        ModelIndex = Random.Range(0, 8); // Randomly pick the styling for the customer from the different varieties.

        _Animator.runtimeAnimatorController = AnimationControllers[ModelIndex]; // Set the animation controller for the customer to the selected style.

        CurrentPosition = transform.position;
    }

    protected void FixedUpdate()
    {
        _RigidBody2D.position = CurrentPosition; // Ensure the customer's position is updated based on the position set externally.
    }

    public virtual void OnEnable()
    {
        if (PatienceMeterAnimator != null && CurrentLocation == Constants.RESTAURANT)
        {
            PatienceMeterAnimator.Play("PatienceStart", 0, 1.0f - (Patience / InitialPatience)); // Ensure if the patience meter needs to be displayed that it is,
            // and that the animation is set to the correct progress based on the current patience level.
        }
    }

    protected virtual void OnDestroy()
    {
        if (PatienceCoroutine != null && Registry.GameManagerObject != null)
        {
            Registry.GameManagerObject.StopCoroutine(PatienceCoroutine); // Stop the coroutine that manages the patience of the customer.
        }
    }

    public void SetupCustomerCoreForRestaurant(int PositionIndex)// Used to set up the customer (both types) for the restaurant scene.
    {
        CurrentPosition = new Vector2(
            Constants.CUSTOMER_SEATS_IN_RESTAURANT[PositionIndex, 0],
            Constants.CUSTOMER_SEATS_IN_RESTAURANT[PositionIndex, 1]); // sit the customer in the right position for that empty space in the restaurant.

        // Change the rendering configuration for the customers so that the player can appear both above/behind them.
        _Renderer.sortingLayerName = "NPC Upper";
        _Renderer.sortingOrder = 1;

        Registry.LevelManagerObject.CustomerTableArrangement[PositionIndex] = gameObject;

        Patience = Random.Range(
            Constants.CUSTOMER_MIN_PATIENCE[Registry.LevelNumber],
            Constants.CUSTOMER_MAX_PATIENCE[Registry.LevelNumber]); // Controls how long the customer will exist in the restaurant before it leaves (when not served)

        InitialPatience = Patience;

        PatienceCoroutine = Registry.GameManagerObject.StartCoroutine(ManagePatience()); // Used to keep track of the lifetime of the customer in the restaurant.

        CustomerTablePosition = PositionIndex; // Stores the seating position for the customer in the restaurant.

        if (PatienceMeter != null)
        {
            PatienceMeter.SetActive(true);
        }
    }

    public IEnumerator ManagePatience()
    {
        if (PatienceMeterAnimator != null)
        {
            PatienceMeterAnimator.speed = 30.017f / InitialPatience; // Determine the patience speed based on how much patience the customer has to begin with (randomly generated).
        }
        while (this != null) // Continue until the customer is garbage collected.
        {
            if (DeSpawn)
            {
                yield break; // Stop when the customer is de-spawned.
            }
            if (Registry.CurrentSceneName == Constants.KITCHEN ||
                    Registry.CurrentSceneName == Constants.RESTAURANT ||
                    Registry.CurrentSceneName == Constants.PHO_MG ||
                    Registry.CurrentSceneName == Constants.RICE_MG ||
                    Registry.CurrentSceneName == Constants.SUSHI_MG ||
                    Registry.CurrentSceneName == Constants.BUNS_MG) // Ensures that customers don't de-spawn in the scenes where the game
                                                                    // is supposed to be paused.
            {
                Patience -= Time.deltaTime * Registry.NotInTutorialScreenTimeModifier;

                if (Patience <= 0) // Flag for garbage collection.
                {
                    DeSpawn = true;

                    if (!gameObject.activeSelf)
                    {
                        Registry.LevelManagerObject.CustomerTableArrangement[CustomerTablePosition] = null;
                        Registry.Customers.Remove(gameObject);
                        Destroy(gameObject);
                    }
                }
            }
            yield return null;
        }
    }
}
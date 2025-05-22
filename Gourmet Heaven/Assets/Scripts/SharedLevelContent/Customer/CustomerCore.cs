using UnityEngine;

// Note: This class is inherited by both varieties of customer, so any reference to 'customer' here refers to both types of customer.

public class CustomerCore : MonoBehaviour
{
    public Animator _Animator;
    public Rigidbody2D _RigidBody2D;
    public Renderer _Renderer;
    public GameObject PatienceMeter;

    public RuntimeAnimatorController[] AnimationControllers = new RuntimeAnimatorController[8]; // Stores each of the 8 different styles for the animations.

    public int ModelIndex;
    public int CustomerTablePosition; // Stores the customer's position in the restaurant (if applicable).
    private int CustomerAnimationState; // Store the current animation state of the customer.

    public bool CustomerSeated = false; // Used to determine if the customer is seated in the restaurant or not.
    public bool DeSpawn = false; // Used to determine when the customer can be garbage collected.

    public float Patience;
    public float InitialPatience;

    public Vector2 CurrentPosition;

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

        ModelIndex = Random.Range(0, 8); // Randomly pick the styling for the customer from the different varieties.

        _Animator.runtimeAnimatorController = AnimationControllers[ModelIndex]; // Set the animation controller for the customer to the selected style.

        CurrentPosition = transform.position;
    }

    protected void FixedUpdate()
    {
        _RigidBody2D.position = CurrentPosition; // Ensure the customer's position is updated based on the position set externally.
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

        CustomerTablePosition = PositionIndex; // Stores the seating position for the customer in the restaurant.

        if (PatienceMeter != null)
        {
            PatienceMeter.SetActive(true);
        }
    }

    public void SetAnimationState(int StateNumber)
    {
        if (_Animator != null) // Set and keep track of the customer's animation state.
        {
            CustomerAnimationState = StateNumber;
            _Animator.SetInteger("customerState", CustomerAnimationState);
        }
    }

    protected void OnEnable()
    {
        if (_Animator != null)
        {
            _Animator.SetInteger("customerState", CustomerAnimationState);
        }
    }

    protected void ManagePatience()
    {
        if (CustomerSeated)
        {
            Patience -= Time.deltaTime * Registry.NotInTutorialScreenTimeModifier;

            if (Patience <= 0) // Flag for garbage collection.
            {
                DeSpawn = true;
            }
        }
    }

    protected void ManagePatience(Animator PatienceMeterAnimator)
    {
        if (CustomerSeated)
        {
            PatienceMeterAnimator.speed = 30.017f / InitialPatience; // Determine the patience speed based on how much patience the customer has to begin with (randomly generated).
            Patience -= Time.deltaTime * Registry.NotInTutorialScreenTimeModifier;

            if (Patience <= 0) // Flag for garbage collection.
            {
                DeSpawn = true;
            }
        }
    }
}
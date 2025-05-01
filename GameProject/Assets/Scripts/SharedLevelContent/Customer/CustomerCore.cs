using System.Collections;
using UnityEngine;

public class CustomerCore : MonoBehaviour
{
    public Animator _Animator;
    public Rigidbody2D _RigidBody2D;
    public Renderer _Renderer;
    public GameObject PatienceMeter;
    private Animator PatienceMeterAnimator;

    public RuntimeAnimatorController[] AnimationControllers = new RuntimeAnimatorController[8];

    public string CurrentLocation = Constants.KITCHEN;

    public int ModelIndex;
    public int CustomerTablePosition;

    public bool DeSpawn = false;

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
        DontDestroyOnLoad(gameObject);

        _Animator = GetComponent<Animator>();
        _RigidBody2D = GetComponent<Rigidbody2D>();
        _Renderer = GetComponent<Renderer>();
        PatienceMeterAnimator = PatienceMeter.GetComponent<Animator>();

        ModelIndex = Random.Range(0, 8);

        _Animator.runtimeAnimatorController = AnimationControllers[ModelIndex];

        CurrentPosition = transform.position;
    }

    protected void FixedUpdate()
    {
        _RigidBody2D.position = CurrentPosition;
    }

    protected virtual void OnDestroy()
    {
        if (PatienceCoroutine != null && Registry.GameManagerObject != null)
        {
            Registry.GameManagerObject.StopCoroutine(PatienceCoroutine);
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

        PatienceMeter.SetActive(true);
    }

    public IEnumerator ManagePatience()
    {
        PatienceMeterAnimator.speed = 30.017f / InitialPatience;
        while (this != null)
        {
            if (DeSpawn)
            {
                yield break;
            }
            if (Registry.CurrentSceneName == Constants.KITCHEN ||
                    Registry.CurrentSceneName == Constants.RESTAURANT ||
                    Registry.CurrentSceneName == Constants.PHO_MG ||
                    Registry.CurrentSceneName == Constants.RICE_MG ||
                    Registry.CurrentSceneName == Constants.SUSHI_MG ||
                    Registry.CurrentSceneName == Constants.BUNS_MG) // Ensures that customers dont despawn in the scenes where the game
                                                                    // is supposed to be paused.
            {
                Patience -= Time.deltaTime;
                if (Patience <= 0)
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
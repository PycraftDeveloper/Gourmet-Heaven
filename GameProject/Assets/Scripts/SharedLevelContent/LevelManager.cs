using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Appliance // This class is used to switch the icons for the mini-games between IDLE and ACTIVATED states.
{
    public Tile Idle;
    public Tile Active;
    private bool State; // False - IDLE state. True - ACTIVE state.

    public void SetState(bool state, bool ConsiderHoldingMeal = true)
    {
        bool InternalState = true;

        if (ConsiderHoldingMeal)
        {
            InternalState = Registry.PlayerObject.HoldingMeal == Constants.NOT_HOLDING_MEAL;
        }
        State = state && InternalState; // Block access to mini-games when player is already holding a meal.
    }

    public void ManageState(Tilemap ApplianceTileMap) // Swaps the tiles around by switching between IDLE/ACTIVE states.
    {
        if (State)
        {
            ApplianceTileMap.SwapTile(Idle, Active);
        }
        else
        {
            ApplianceTileMap.SwapTile(Active, Idle);
        }
    }

    public bool GetState()
    {
        return State;
    }
}

public class LevelManager : MonoBehaviour
{
    public GameObject CustomerPrefab; // Used to spawn customers that CAN place orders in the kitchen
    public GameObject BackgroundCustomerPrefab; // Used to spawn filler customers in the restaurant.

    // Below are the tiles that can be switched between IDLE and ACTIVE states.
    public Appliance CacheRegister;

    public Appliance Bin;

    public Appliance ChoppingBoard;
    public Appliance PhoBowl;
    public Appliance CookingPot;
    public Appliance SushiRollingMat;

    public TextMeshProUGUI UIScoreText;
    public TextMeshProUGUI UIDayText;

    // Below are the two tilemaps that create the illusion of depth. ONLY the CacheRegister is in the 'AboveApplianceTileMap'.
    public Tilemap AboveApplianceTileMap;

    public Tilemap BehindApplianceTileMap;

    // Defines the off-screen location at which Kitchen customers spawn at (hidden from view).
    private Vector2 CustomerSpawningLocation = new Vector2(9.47f, -3.61f);

    private float CustomerSpawnTimer = 0; // Controls when the next customer should spawn on the schedule.
    private float NextCustomerSpawnTime = 0; // Stores the values the 'CustomerSpawnTimer' needs to reach before a new customer is spawned (UNLESS there are no customers in the kitchen).
    public int CustomersInScene = 0; // Stores a running total of the number of customers currently in the game.

    public Queue<GameObject> CustomerKitchenQueue = new Queue<GameObject>(); // Stores the order customers are arranged in the Kitchen queue.
    public GameObject[] CustomerTableArrangement = new GameObject[8]; // Stores the seating positions and where customers are currently seated in the restaurant scene.

    private bool ReturnToGameToggle = true; // Used to trigger a unique event when the player returns to the game scenes (Kitchen/Restaurant) from another menu, like the pause menu.

    private void Start()
    {
        UIDayText.text = (Registry.LevelNumber + 1).ToString();
    }

    private void GenerateBackgroundCustomers(int MaxQuantity) // Used to generate random numbers of background customers for the restaurant scene.
    {
        int FreeSpaces = 1; // There will always be a minimum of one free space in the restaurant as this is called on start-up, and when another background customer is removed.
        foreach (GameObject _Customer in CustomerTableArrangement)
        {
            if (_Customer == null)
            {
                FreeSpaces++; // Count the number of free spaces in the scene, so the code knows how many background customers will be possible to spawn.
            }
        }

        int NumberOfBackgroundCustomers = Random.Range(0, Mathf.Min(MaxQuantity, FreeSpaces)); // Randomly generate n background customers up to the limit where there are no free spaces left.
        for (int i = 0; i < NumberOfBackgroundCustomers; i++)
        {
            GameObject NewBackgroundCustomer = Instantiate(BackgroundCustomerPrefab, transform.position, transform.rotation);

            bool Seated = false;
            while (!Seated) // Search through all the seats in the restaurant until there is one available.
            {
                int PositionIndex = Random.Range(0, 8); // Pick a position randomly instead of iterating linearly through the available seats to mimmick a more realistic
                // distribution of customers in the restaurant.

                if (CustomerTableArrangement[PositionIndex] == null) // When a free seat is found.
                {
                    BackgroundCustomer _BackgroundCustomer = NewBackgroundCustomer.GetComponent<BackgroundCustomer>();
                    if (PositionIndex % 2 == 0) // If the seat is facing the other way, flip the sprite!
                    {
                        Vector3 CustomerScale = _BackgroundCustomer.transform.localScale;
                        CustomerScale.x *= -1;
                        _BackgroundCustomer.transform.localScale = CustomerScale;
                    }

                    Registry.Customers.Add(NewBackgroundCustomer); // Add the background customer to the centralised tracking system.
                    _BackgroundCustomer.SetupCustomerCoreForRestaurant(_BackgroundCustomer, PositionIndex); // Set-up the background customer for the restaurant.
                    NewBackgroundCustomer.SetActive(false); // By default, don't show the new customer until the scene is changed (so the customers don't pop in when player is in scene)
                    Seated = true;
                }
            }
        }
    }

    private void Awake() // This program ensures that at any time there is a maximum of one scene manager in the game, as they aren't destoyed when changing scenes.
    {
        if (Registry.LevelManagerObject == null)
        {
            DontDestroyOnLoad(gameObject);
            Registry.LevelManagerObject = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void HandleOrderCollection()
    {
        for (int i = 0; i < Registry.Customers.Count; i++)
        {
            Customer _Customer = Registry.Customers[i].GetComponent<Customer>();
            if (_Customer != null)
            {
                if (_Customer.CurrentLocation == Constants.KITCHEN)
                {
                    if (_Customer.CurrentPosition.x == 0.5f)
                    {
                        CustomerKitchenQueue.Dequeue();
                        _Customer.MealPlaced = true;
                        _Customer.SetCoroutine(_Customer.MoveIntoRestaurant(), Constants.MOVE_INTO_RESTAURANT);
                        Invoke("UpdateQueuePositions", 1.5f);
                        Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.CashRegisterNoise); // added by Joshua Cossar
                        return;
                    }
                }
            }
        }
    }

    private void HandleApplianceState()
    {
        if (AboveApplianceTileMap == null)
        {
            AssociateApplianceTilemap();
            return;
        }

        CacheRegister.ManageState(AboveApplianceTileMap);
        ChoppingBoard.ManageState(BehindApplianceTileMap);
        CookingPot.ManageState(BehindApplianceTileMap);
        PhoBowl.ManageState(BehindApplianceTileMap);
        SushiRollingMat.ManageState(BehindApplianceTileMap);
        Bin.ManageState(BehindApplianceTileMap);
    }

    private void AssociateApplianceTilemap()
    {
        Tilemap[] tilemaps = FindObjectsByType<Tilemap>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (Tilemap tilemap in tilemaps)
        {
            if (tilemap.gameObject.name == "MiscAboveTilemap")
            {
                AboveApplianceTileMap = tilemap.GetComponent<Tilemap>();
            }
            else if (tilemap.gameObject.name == "MiscBehindTilemap")
            {
                BehindApplianceTileMap = tilemap.GetComponent<Tilemap>();
            }

            if (AboveApplianceTileMap != null && BehindApplianceTileMap != null)
            {
                break;
            }
        }
        UpdateQueuePositions();
    }

    private void UpdateQueuePositions()
    {
        float spacing = 1.5f;
        int index = 0;

        foreach (GameObject obj in CustomerKitchenQueue)
        {
            Customer customer = obj.GetComponent<Customer>();
            Vector2 DestinationPosition = new Vector2(0.5f + spacing * index, -3.61f);
            customer.SetCoroutine(customer.MoveInQueue(DestinationPosition), Constants.MOVE_IN_QUEUE);
            index++;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Registry.InGameLevel == false)
        {
            Registry.GameTimeDelta = 0;
            ReturnToGameToggle = true;
            return;
        }

        UIScoreText.text = Registry.PlayerScore.ToString();

        CacheRegister.SetState(false);
        ChoppingBoard.SetState(false);
        CookingPot.SetState(false);
        PhoBowl.SetState(false);
        SushiRollingMat.SetState(false);
        Bin.SetState(false);

        Bin.SetState(Registry.PlayerObject.HoldingMeal != Constants.NOT_HOLDING_MEAL, false);

        if (ReturnToGameToggle)
        {
            AssociateApplianceTilemap();
        }
        Registry.GameTimeDelta = Time.deltaTime;
        ReturnToGameToggle = false;

        CustomerSpawnTimer += Registry.GameTimeDelta;

        for (int i = 0; i < Registry.Customers.Count; i++)
        {
            Customer thisCustomer = Registry.Customers[i].GetComponent<Customer>();

            if (thisCustomer == null)
            {
                continue;
            }

            if (thisCustomer.MealPlaced)
            {
                switch (thisCustomer.Meal)
                {
                    case Constants.PHO:
                        PhoBowl.SetState(true);
                        break;

                    case Constants.SUSHI:
                        SushiRollingMat.SetState(true);
                        break;

                    case Constants.BAO_BUNS:
                        CookingPot.SetState(true);
                        break;

                    case Constants.MANGO_STICKY_RICE:
                        ChoppingBoard.SetState(true);
                        break;
                }
            }

            if (Registry.CurrentSceneName == Constants.KITCHEN)
            {
                if (thisCustomer.CurrentPosition.x == 0.5f && thisCustomer.CurrentPosition.y == -3.61f && CustomersInScene - CustomerKitchenQueue.Count < 8)
                {
                    thisCustomer.SetAnimationState(Constants.CUSTOMER_IDLE_UP_ANIMATION);
                    CacheRegister.SetState(true);
                }
            }
            else if (thisCustomer.CurrentLocation == Constants.RESTAURANT)
            {
                thisCustomer.SetAnimationState(Constants.CUSTOMER_IDLE_SIT_ANIMATION);
            }
        }

        if (CustomerKitchenQueue.Count == 0 || CustomerSpawnTimer > NextCustomerSpawnTime)
        {
            if (CustomerKitchenQueue.Count == 0)
            {
                GenerateBackgroundCustomers(9);
            }
            else
            {
                GenerateBackgroundCustomers(Random.Range(2, 6));
            }

            NextCustomerSpawnTime = Random.Range(Constants.CUSTOMER_MIN_SPAWN_RATE[Registry.LevelNumber], Constants.CUSTOMER_MAX_SPAWN_RATE[Registry.LevelNumber]);
            CustomerSpawnTimer = 0;
            if (CustomerKitchenQueue.Count < 6 && CustomersInScene < 14)
            {
                GameObject NewCustomer = Instantiate(CustomerPrefab, CustomerSpawningLocation, transform.rotation);
                if (Registry.CurrentSceneName != Constants.KITCHEN)
                {
                    NewCustomer.SetActive(false);
                }
                CustomerKitchenQueue.Enqueue(NewCustomer);
                Registry.Customers.Add(NewCustomer);
                UpdateQueuePositions();
                CustomersInScene++;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) // Pause the game when the user its the back button.
        {
            Registry.GameManagerObject.ChangeScene(Constants.PAUSE_MENU); // Go to the Pause Menu scene.
        }

        HandleApplianceState();
    }

    private void OnApplicationFocus(bool focus) // Pause the game when the user puts the game in the background. ONLY when in built version of the game
    {
        if (!focus && !Application.isEditor && Registry.InGameLevel) // if NOT in focus
        {
            Registry.GameManagerObject.RenderGameSceneToFrameBuffer(); // needs to be called immediately
            Registry.GameManagerObject.ChangeScene(Constants.PAUSE_MENU); // Go to the Pause Menu scene.
        }
    }
}
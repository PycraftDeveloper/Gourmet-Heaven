using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
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

    public AudioClip BackgroundMusic;

    // Below are the two tile-maps that create the illusion of depth. ONLY the CacheRegister is in the 'AboveApplianceTileMap'.
    public Tilemap AboveApplianceTileMap;

    public Tilemap BehindApplianceTileMap;

    // Defines the off-screen location at which Kitchen customers spawn at (hidden from view).
    private Vector2 CustomerSpawningLocation = new Vector2(9.47f, -3.61f);

    private float CustomerSpawnTimer = 0; // Controls when the next customer should spawn on the schedule.
    private float NextCustomerSpawnTime = 0; // Stores the values the 'CustomerSpawnTimer' needs to reach before a new customer is spawned (UNLESS there are no customers in the kitchen).
    public int CustomersInScene = 0; // Stores a running total of the number of customers currently in the game.

    public Queue<ForegroundCustomer> CustomerKitchenQueue = new Queue<ForegroundCustomer>(); // Stores the order customers are arranged in the Kitchen queue.
    public GameObject[] CustomerTableArrangement = new GameObject[8]; // Stores the seating positions and where customers are currently seated in the restaurant scene.

    private bool ReturnToGameToggle = true; // Used to trigger a unique event when the player returns to the game scenes (Kitchen/Restaurant) from another menu, like the pause menu.

    private void Start()
    {
        if (Registry.CoreGameInfrastructureObject.GameMusicSource.clip != BackgroundMusic)
        {
            Registry.CoreGameInfrastructureObject.GameMusicSource.clip = BackgroundMusic;
            Registry.CoreGameInfrastructureObject.GameMusicSource.loop = true;
            Registry.CoreGameInfrastructureObject.GameMusicSource.Play();
        }

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
            GameObject NewBackgroundCustomer = Instantiate(BackgroundCustomerPrefab, CustomerSpawningLocation, transform.rotation);

            bool Seated = false;
            while (!Seated) // Search through all the seats in the restaurant until there is one available.
            {
                int PositionIndex = Random.Range(0, 8); // Pick a position randomly instead of iterating linearly through the available seats to mimic a more realistic
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
                    _BackgroundCustomer.CustomerTablePosition = PositionIndex;
                    Registry.BackgroundCustomers.Add(_BackgroundCustomer); // Add the background customer to the centralised tracking system.
                    _BackgroundCustomer.SetupCustomerCoreForRestaurant(PositionIndex); // Set-up the background customer for the restaurant.
                    Seated = true;
                }
            }
        }
    }

    private void Awake() // This program ensures that at any time there is a maximum of one scene manager in the game, as they aren't destroyed when changing scenes.
    {
        Registry.LevelManagerObject = this;

        CinemachineCamera GameLevelCinemachineCamera = FindFirstObjectByType<CinemachineCamera>(); // Find the Cinemachine camera in the scene.
        if (GameLevelCinemachineCamera != null && Registry.PlayerObject != null)
        {
            CameraTarget CinemachineCameraTarget = new CameraTarget(); // Get the camera target from the Cinemachine camera.
            CinemachineCameraTarget.TrackingTarget = Registry.PlayerObject.transform; // Set the camera target to the player object.
            GameLevelCinemachineCamera.Target = CinemachineCameraTarget;
        }

        Canvas[] UICanvases = FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None); // Find the UI canvas in the scene.
        foreach (Canvas UICanvas in UICanvases)
        {
            if (UICanvas.gameObject.name == "PopUpCanvas") // Find the Game UI canvas.
            {
                UICanvas.worldCamera = Camera.main; // Set the camera for the UI canvas to the main camera.
                break;
            }
        }
    }

    public void HandleOrderCollection() // Runs when the player collects an order from the customer standing at the cache machine.
    {
        for (int i = 0; i < Registry.ForegroundCustomers.Count; i++) // Iterate over all the customers.
        {
            ForegroundCustomer _Customer = Registry.ForegroundCustomers[i];
            if (_Customer != null)
            {
                if (_Customer.CurrentPosition.x == 0.5f && _Customer.CurrentPosition.y == -3.61f) // If the customer is in the correct location and is in the position in the queue at the till point.
                {
                    CustomerKitchenQueue.Dequeue(); // Remove the first customer from the queue.
                    _Customer.MealPlaced = true;
                    _Customer.SetCoroutine(_Customer.MoveIntoRestaurant(), Constants.MOVE_INTO_RESTAURANT); // Set-up the customer to move down out of the kitchen scene.
                    Invoke("UpdateQueuePositions", 1.5f); // Once the customer has left the scene, update the positions for the other customers in the queue.
                    Registry.CoreGameInfrastructureObject.SFXSource.PlayOneShot(Registry.CoreGameInfrastructureObject.CashRegisterNoise); // added by Joshua Cossar
                    return; // Early exit the loop.
                }
            }
        }
    }

    private void HandleApplianceState() // This ensures the appliances correctly change to show if they are interactable or not.
    {
        if (AboveApplianceTileMap == null)
        {
            AssociateApplianceTilemap(); // Ensure that the appliance tile map has been assigned.
            return;
        }

        // Manage the states for each of the appliances in the scene.
        CacheRegister.ManageState(AboveApplianceTileMap);
        ChoppingBoard.ManageState(BehindApplianceTileMap);
        CookingPot.ManageState(BehindApplianceTileMap);
        PhoBowl.ManageState(BehindApplianceTileMap);
        SushiRollingMat.ManageState(BehindApplianceTileMap);
        Bin.ManageState(BehindApplianceTileMap);
    }

    private void AssociateApplianceTilemap() // Ensure that the level manager has the correct tile maps assigned to it when in the kitchen scenes for the appliances.
    {
        Tilemap[] tilemaps = FindObjectsByType<Tilemap>(FindObjectsInactive.Include, FindObjectsSortMode.None); // Iterate over all the tile maps in the scene.

        foreach (Tilemap tilemap in tilemaps)
        {
            if (tilemap.gameObject.name == "MiscAboveTilemap") // Find based on name
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
        UpdateQueuePositions(); // Ensure the customers are correctly positioned when the scene is changed.
    }

    private void UpdateQueuePositions()
    {
        float spacing = 1.5f; // How far the customers should be spaced.
        int index = 0;

        foreach (ForegroundCustomer CustomerObject in CustomerKitchenQueue) // Iterate over each of the customers.
        {
            Vector2 DestinationPosition = new Vector2(0.5f + spacing * index, -3.61f); // Update the destination position
            CustomerObject.SetCoroutine(CustomerObject.MoveInQueue(DestinationPosition), Constants.MOVE_IN_QUEUE); // Start the coroutine that will move the customers in the background.
            index++;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        UIScoreText.text = Registry.PlayerScore.ToString(); // Update the score text in the UI.

        // Reset all the appliance states (so they can be enabled later, or left disabled).
        // Note these set states ensure the appliances are not interactable when the player is holding a meal.
        CacheRegister.SetState(false);
        ChoppingBoard.SetState(false);
        CookingPot.SetState(false);
        PhoBowl.SetState(false);
        SushiRollingMat.SetState(false);
        Bin.SetState(false);

        // Allows the bin to always be available when the player is holding a meal, and does NOT take into account internally if the player is holding a meal.
        Bin.SetState(Registry.PlayerObject.HoldingMeal != Constants.NOT_HOLDING_MEAL, false);

        if (ReturnToGameToggle)
        {
            AssociateApplianceTilemap(); // If this is the first frame back after the scene has returned to a game scene.
        }
        Registry.GameTimeDelta = Time.deltaTime * Registry.NotInTutorialScreenTimeModifier; // Update the time delta used by the game. This allows for time to 'freeze' when in a tutorial
        // or when not in the game scenes.
        ReturnToGameToggle = false; // Set the toggle to false so it doesn't run again until the scene changes.

        CustomerSpawnTimer += Registry.GameTimeDelta;

        for (int i = 0; i < Registry.ForegroundCustomers.Count; i++) // iterate over each of the customers in the game scene, used to determine appliance states
        {
            ForegroundCustomer thisCustomer = Registry.ForegroundCustomers[i];

            if (thisCustomer == null)
            {
                continue;
            }

            if (thisCustomer.MealPlaced)
            {
                switch (thisCustomer.Meal) // For each customer waiting for a meal to be served, update which appliances should be 'active'.
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

            if (thisCustomer.CurrentPosition.x == 0.5f && thisCustomer.CurrentPosition.y == -3.61f && CustomersInScene - CustomerKitchenQueue.Count < 8) // Check if the customer is in
                                                                                                                                                         // the kitchen scene, and that it is waiting at the till point.
            {
                thisCustomer.SetAnimationState(Constants.CUSTOMER_IDLE_UP_ANIMATION); // Set the customer to idle facing the till point.
                CacheRegister.SetState(true); // 'activate' the till point tile
            }
        }

        if (Registry.PlayerObject.transform.position.y > -2.4 && (CustomerKitchenQueue.Count == 0 || CustomerSpawnTimer > NextCustomerSpawnTime)) // If there are no customers waiting to be served, or it's time to spawn a new customer in the queue.
        {
            if (CustomerKitchenQueue.Count == 0)
            {
                GenerateBackgroundCustomers(9); // If there are no customers in the queue, ensure all the tables in the restaurant are filled with background customers.
            }
            else
            {
                GenerateBackgroundCustomers(Random.Range(2, 6)); // If there are customers in the queue, randomly generate between 2 and 6 background customers.
            }

            NextCustomerSpawnTime = Random.Range(Constants.CUSTOMER_MIN_SPAWN_RATE[Registry.LevelNumber], Constants.CUSTOMER_MAX_SPAWN_RATE[Registry.LevelNumber]); // Reset the customer spawn timer.
            CustomerSpawnTimer = 0;
            if (CustomerKitchenQueue.Count < 6 && CustomersInScene < 14) // If there is space in the kitchen queue to spawn more customers - and also ensure that there are less than 15 customers in the scene at
                                                                         // any given time.
            {
                // Spawn and set-up a new customer for the kitchen queue.
                GameObject NewCustomer = Instantiate(CustomerPrefab, CustomerSpawningLocation, transform.rotation);
                ForegroundCustomer NewForegroundCustomer = NewCustomer.GetComponent<ForegroundCustomer>();
                CustomerKitchenQueue.Enqueue(NewForegroundCustomer);
                Registry.ForegroundCustomers.Add(NewForegroundCustomer);
                UpdateQueuePositions();
                CustomersInScene++;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !Registry.GamePaused && !Registry.InMiniGame) // Pause the game when the user its the back button.
        {
            Registry.GamePaused = true;
            Registry.CoreGameInfrastructureObject.RenderGameSceneToFrameBuffer();
            Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.PAUSE_MENU); // Go to the Pause Menu scene.
        }

        HandleApplianceState();

        if (Registry.LevelRunTime > 0)
        {
            Registry.LevelRunTime -= Time.deltaTime;
            if (Registry.LevelRunTime <= 0)
            {
                Registry.CoreGameInfrastructureObject.ChangeScene(Constants.MENU_SCENE);
                Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.END_MENU);
            }
        }
    }

    private void OnApplicationFocus(bool focus) // Pause the game when the user puts the game in the background. ONLY when in built version of the game
    {
        if (!focus && !Application.isEditor && !Registry.InMiniGame) // if NOT in focus
        {
            Registry.CoreGameInfrastructureObject.RenderGameSceneToFrameBuffer(); // needs to be called immediately
            Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.PAUSE_MENU); // Go to the Pause Menu scene.
        }
    }
}
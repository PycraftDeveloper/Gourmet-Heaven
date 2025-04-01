using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Appliance
{
    public Tile Idle;
    public Tile Active;
    private bool State;

    public void SetState(bool state)
    {
        State = state && Registry.PlayerObject.HoldingMeal == Constants.NOT_HOLDING_MEAL;
    }

    public void ManageState(Tilemap ApplianceTileMap)
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
    public GameObject CustomerPrefab;
    public GameObject BackgroundCustomerPrefab;

    public Appliance CacheRegister;
    public Appliance ChoppingBoard;
    public Appliance PhoBowl;
    public Appliance CookingPot;
    public Appliance SushiRollingMat;

    public Tilemap AboveApplianceTileMap;
    public Tilemap BehindApplianceTileMap;

    private Vector2 CustomerSpawningLocation = new Vector2(9.47f, -3.61f);

    private float CustomerSpawnTimer = 0;
    private float NextCustomerSpawnTime = 0;

    private Queue<GameObject> CustomerKitchenQueue = new Queue<GameObject>();
    public GameObject[] CustomerTableArrangement = new GameObject[8];

    private bool ReturnToGameToggle = true;

    private void SetupCustomerCoreForRestaurant(CustomerCore _CustomerCore, GameObject _Customer, int PositionIndex)
    {
        _CustomerCore.CurrentPosition = new Vector2(Constants.CUSTOMER_SEATS_IN_RESTAURANT[PositionIndex, 0], Constants.CUSTOMER_SEATS_IN_RESTAURANT[PositionIndex, 1]);
        _CustomerCore._Renderer.sortingLayerName = "NPC Upper";
        _CustomerCore._Renderer.sortingOrder = 1;
        CustomerTableArrangement[PositionIndex] = _Customer;
        _CustomerCore.Patience = Random.Range(Constants.CUSTOMER_MIN_PATIENCE[Registry.LevelNumber], Constants.CUSTOMER_MAX_PATIENCE[Registry.LevelNumber]);
        Registry.GameManagerObject.StartCoroutine(_CustomerCore.ManagePatience());
        _CustomerCore.CustomerTablePosition = PositionIndex;
    }

    private void GenerateBackgroundCustomers(int MaxQuantity)
    {
        int FreeSpaces = 1;
        foreach (GameObject _Customer in CustomerTableArrangement)
        {
            if (_Customer == null)
            {
                FreeSpaces++;
            }
        }

        int NumberOfBackgroundCustomers = Random.Range(0, Mathf.Min(MaxQuantity, FreeSpaces));
        for (int i = 0; i < NumberOfBackgroundCustomers; i++)
        {
            GameObject NewBackgroundCustomer = Instantiate(BackgroundCustomerPrefab, transform.position, transform.rotation);

            bool Seated = false;
            while (!Seated)
            {
                int PositionIndex = Random.Range(0, 8);
                if (CustomerTableArrangement[PositionIndex] == null)
                {
                    BackgroundCustomer _BackgroundCustomer = NewBackgroundCustomer.GetComponent<BackgroundCustomer>();
                    CustomerCore _CustomerCore = NewBackgroundCustomer.GetComponent<CustomerCore>();
                    if (PositionIndex % 2 == 0)
                    {
                        Vector3 CustomerScale = _BackgroundCustomer.transform.localScale;
                        CustomerScale.x *= -1;
                        _BackgroundCustomer.transform.localScale = CustomerScale;
                    }
                    Registry.Customers.Add(NewBackgroundCustomer);
                    SetupCustomerCoreForRestaurant(_CustomerCore, NewBackgroundCustomer, PositionIndex);
                    NewBackgroundCustomer.SetActive(false);
                    Seated = true;
                }
            }
        }
    }

    private void Awake()
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

    private void PlaceIntoRestaurant(GameObject CustomerGameObject)
    {
        bool Seated = false;
        while (!Seated)
        {
            int PositionIndex = Random.Range(0, 8);
            if (CustomerTableArrangement[PositionIndex] == null || CustomerTableArrangement[PositionIndex].GetComponent<BackgroundCustomer>())
            {
                Customer _Customer = CustomerGameObject.GetComponent<Customer>();
                CustomerCore _CustomerCore = CustomerGameObject.GetComponent<CustomerCore>();
                if (PositionIndex % 2 == 0)
                {
                    Vector3 CustomerScale = _Customer.transform.localScale;
                    CustomerScale.x *= -1;
                    _Customer.transform.localScale = CustomerScale;
                }
                _Customer.SetAnimationState(Constants.CUSTOMER_IDLE_SIDE_ANIMATION); // update with restaurant animation later
                SetupCustomerCoreForRestaurant(_CustomerCore, CustomerGameObject, PositionIndex);
                Seated = true;
            }
        }
        if (Registry.CurrentSceneName != Constants.RESTAURANT)
        {
            CustomerGameObject.SetActive(false);
        }
    }

    private IEnumerator MoveIntoRestaurant(Customer _Customer, GameObject CustomerGameObject)
    {
        _Customer.SetAnimationState(Constants.CUSTOMER_WALK_DOWN_ANIMATION);

        while (_Customer._CustomerCore.CurrentLocation == Constants.KITCHEN && _Customer.gameObject != null)
        {
            _Customer._CustomerCore.CurrentPosition = new Vector2(_Customer._CustomerCore.CurrentPosition.x, _Customer._CustomerCore.CurrentPosition.y - 0.1f);

            if (_Customer._CustomerCore.CurrentPosition.y < -6.31 || Registry.InGameLevel == false)
            {
                _Customer._CustomerCore.CurrentLocation = Constants.RESTAURANT;
            }
            yield return null;
        }
        PlaceIntoRestaurant(CustomerGameObject);
    }

    public void HandleOrderCollection()
    {
        for (int i = 0; i < Registry.Customers.Count; i++)
        {
            Customer customer = Registry.Customers[i].GetComponent<Customer>();
            if (customer._CustomerCore.CurrentLocation == Constants.KITCHEN)
            {
                if (customer._CustomerCore.CurrentPosition.x == 0.5f)
                {
                    CustomerKitchenQueue.Dequeue();
                    customer.MealPlaced = true;
                    customer.SetCoroutine(MoveIntoRestaurant(customer, Registry.Customers[i]), Constants.MOVE_INTO_RESTAURANT);
                    Invoke("UpdateQueuePositions", 1.5f);
                    return;
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
        ChoppingBoard.ManageState(AboveApplianceTileMap);
        CookingPot.ManageState(BehindApplianceTileMap);
        PhoBowl.ManageState(BehindApplianceTileMap);
        SushiRollingMat.ManageState(AboveApplianceTileMap);
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
            customer.SetCoroutine(MoveInQueue(customer, DestinationPosition), Constants.MOVE_IN_QUEUE);
            index++;
        }
    }

    private IEnumerator MoveInQueue(Customer _Customer, Vector2 DestinationPosition)
    {
        float duration = 2.0f;
        Vector2 start = _Customer._CustomerCore.CurrentPosition;
        float elapsed = 0f;

        if (start.x != DestinationPosition.x)
        {
            _Customer.SetAnimationState(Constants.CUSTOMER_WALK_SIDE_ANIMATION);

            while (elapsed < duration && Registry.InGameLevel == true && _Customer.gameObject != null)
            {
                _Customer._CustomerCore.CurrentPosition = Vector2.Lerp(start, DestinationPosition, elapsed / duration);
                elapsed += Registry.GameTimeDelta;
                yield return null;
            }

            _Customer._CustomerCore.CurrentPosition = DestinationPosition;
            _Customer.SetAnimationState(Constants.CUSTOMER_IDLE_SIDE_ANIMATION);
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

        CacheRegister.SetState(false);
        ChoppingBoard.SetState(false);
        CookingPot.SetState(false);
        PhoBowl.SetState(false);
        SushiRollingMat.SetState(false);

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
                if (thisCustomer._CustomerCore.CurrentPosition.x == 0.5f && thisCustomer._CustomerCore.CurrentPosition.y == -3.61f && Registry.Customers.Count - CustomerKitchenQueue.Count < 8)
                {
                    thisCustomer.SetAnimationState(Constants.CUSTOMER_IDLE_UP_ANIMATION);
                    CacheRegister.SetState(true);
                }
            }
            else if (Registry.CurrentSceneName == Constants.RESTAURANT)
            {
                thisCustomer.SetAnimationState(Constants.CUSTOMER_IDLE_SIDE_ANIMATION); // update with restaurant animation later
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
            if (CustomerKitchenQueue.Count < 6 && Registry.Customers.Count < 14)
            {
                GameObject NewCustomer = Instantiate(CustomerPrefab, CustomerSpawningLocation, transform.rotation);
                if (Registry.CurrentSceneName != Constants.KITCHEN)
                {
                    NewCustomer.SetActive(false);
                }
                CustomerKitchenQueue.Enqueue(NewCustomer);
                Registry.Customers.Add(NewCustomer);
                UpdateQueuePositions();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Registry.GameManagerObject.ChangeScene(Constants.PAUSE_MENU);
        }

        HandleApplianceState();
    }
}
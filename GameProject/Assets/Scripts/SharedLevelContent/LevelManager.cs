using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        State = state;
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
    private GameObject[] CustomerTableArrangement = new GameObject[8];

    private bool ReturnToGameToggle = true;

    private void Start()
    {
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
        if (Registry.CurrentSceneName != Constants.RESTAURANT)
        {
            CustomerGameObject.SetActive(false);
        }
        bool Seated = false;
        while (!Seated)
        {
            int PositionIndex = Random.Range(0, 8);
            if (CustomerTableArrangement[PositionIndex] == null)
            {
                Customer _Customer = CustomerGameObject.GetComponent<Customer>();
                _Customer.CurrentPosition = new Vector2(Constants.CUSTOMER_SEATS_IN_RESTAURANT[PositionIndex, 0], Constants.CUSTOMER_SEATS_IN_RESTAURANT[PositionIndex, 1]);
                Renderer CustomerRenderer = CustomerGameObject.GetComponent<Renderer>();
                CustomerRenderer.sortingLayerName = "NPC Upper";
                CustomerRenderer.sortingOrder = 1;
                _Customer.Facing = Constants.FACE_SIDE;
                if (PositionIndex % 2 == 0)
                {
                    Vector3 CustomerScale = _Customer.transform.localScale;
                    CustomerScale.x *= -1;
                    _Customer.transform.localScale = CustomerScale;
                }
                CustomerTableArrangement[PositionIndex] = CustomerGameObject;
                Seated = true;
            }
        }
    }

    private IEnumerator MoveIntoRestaurant(Customer _Customer, GameObject CustomerGameObject)
    {
        _Customer.Facing = Constants.FACE_DOWN;

        _Customer.SetState(2);

        while (_Customer.CurrentLocation == Constants.KITCHEN)
        {
            _Customer.Facing = Constants.FACE_DOWN;
            _Customer.CurrentPosition = new Vector2(_Customer.CurrentPosition.x, _Customer.CurrentPosition.y - 0.1f);

            if (_Customer.CurrentPosition.y < -6.31 || Registry.InGameLevel == false)
            {
                _Customer.CurrentLocation = Constants.RESTAURANT;
            }
            yield return null;
        }
        _Customer.SetState(Constants.CUSTOMER_IDLE_SIDE_ANIMATION); // update with restaurant animation later
        PlaceIntoRestaurant(CustomerGameObject);
    }

    public void HandleOrderCollection()
    {
        for (int i = 0; i < Registry.Customers.Count; i++)
        {
            Customer customer = Registry.Customers[i].GetComponent<Customer>();
            if (customer.CurrentLocation == Constants.KITCHEN)
            {
                if (customer.CurrentPosition.x == 0.5f)
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
        Vector2 start = _Customer.CurrentPosition;
        float elapsed = 0f;

        if (start.x != DestinationPosition.x)
        {
            _Customer.SetState(Constants.CUSTOMER_WALK_SIDE_ANIMATION);

            while (elapsed < duration)
            {
                if (Registry.InGameLevel == false)
                {
                    _Customer.SetState(Constants.CUSTOMER_IDLE_SIDE_ANIMATION);
                    yield break;
                }

                _Customer.CurrentPosition = Vector2.Lerp(start, DestinationPosition, elapsed / duration);
                elapsed += Registry.GameTimeDelta;
                yield return null;
            }

            _Customer.CurrentPosition = DestinationPosition;
            _Customer.SetState(Constants.CUSTOMER_IDLE_SIDE_ANIMATION);
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
                if (thisCustomer.CurrentPosition.x == 0.5f && thisCustomer.CurrentPosition.y == -3.61f && Registry.Customers.Count - CustomerKitchenQueue.Count < 8)
                {
                    thisCustomer.Facing = Constants.FACE_UP;
                    thisCustomer.SetState(Constants.CUSTOMER_IDLE_UP_ANIMATION);
                    CacheRegister.SetState(true);
                }
            }

            if (Registry.CurrentSceneName != thisCustomer.CurrentLocation)
            {
                Registry.Customers[i].SetActive(false);
            }
        }

        if (CustomerKitchenQueue.Count == 0 || CustomerSpawnTimer > NextCustomerSpawnTime)
        {
            NextCustomerSpawnTime = Random.Range(5, 15);
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
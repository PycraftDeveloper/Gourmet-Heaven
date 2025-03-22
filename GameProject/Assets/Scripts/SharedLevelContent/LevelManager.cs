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

    private IEnumerator MoveIntoRestaurant(Customer customer)
    {
        Rigidbody2D CustomerRigidBody = customer.GetComponent<Rigidbody2D>();
        customer.Facing = Constants.FACE_DOWN;
        while (true)
        {
            customer.Facing = Constants.FACE_DOWN;
            CustomerRigidBody.position = new Vector2(CustomerRigidBody.position.x, CustomerRigidBody.position.y - 0.1f);

            if (CustomerRigidBody.position.y < -6.31 || Registry.CurrentSceneName != Constants.KITCHEN)
            {
                customer.CurrentLocation = Constants.RESTAURANT;
                yield break;
            }
            yield return null;
        }
    }

    public void HandleOrderCollection()
    {
        for (int i = 0; i < Registry.Customers.Count; i++)
        {
            Customer customer = Registry.Customers[i].GetComponent<Customer>();
            if (customer.CurrentLocation == Constants.KITCHEN)
            {
                if (customer.CustomerRigidBody.position.x == 0.5f)
                {
                    CustomerKitchenQueue.Dequeue();
                    customer.MealPlaced = true;
                    customer.SetCoroutine(MoveIntoRestaurant(customer), Constants.MOVE_INTO_RESTAURANT);
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
            customer.SetCoroutine(MoveInQueue(obj.GetComponent<Rigidbody2D>(), DestinationPosition), Constants.MOVE_IN_QUEUE);
            index++;
        }
    }

    private IEnumerator MoveInQueue(Rigidbody2D CustomerRigidBody, Vector2 DestinationPosition)
    {
        float duration = 2.0f;
        Vector2 start = CustomerRigidBody.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (Registry.CurrentSceneName != Constants.KITCHEN)
            {
                yield break;
            }

            CustomerRigidBody.position = Vector2.Lerp(start, DestinationPosition, elapsed / duration);
            elapsed += Registry.GameTimeDelta;
            yield return null;
        }

        CustomerRigidBody.position = DestinationPosition;
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

        int number_of_customers_in_kitchen = 0;

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
                if (thisCustomer.CurrentLocation == Constants.RESTAURANT)
                {
                    thisCustomer.CustomerSprite.enabled = false;
                }
                else
                {
                    thisCustomer.CustomerSprite.enabled = true;

                    number_of_customers_in_kitchen++;

                    if (thisCustomer.CustomerRigidBody.position.x == 0.5f && thisCustomer.CustomerRigidBody.position.y == -3.61f)
                    {
                        thisCustomer.Facing = Constants.FACE_UP;
                        CacheRegister.SetState(true);
                    }
                }
            }
            else
            {
                if (thisCustomer.CurrentLocation == Constants.KITCHEN)
                {
                    thisCustomer.CustomerSprite.enabled = false;
                }
                else
                {
                    thisCustomer.CustomerSprite.enabled = true;
                }
            }
        }

        if (Registry.CurrentSceneName == Constants.KITCHEN)
        {
            if (number_of_customers_in_kitchen == 0 || CustomerSpawnTimer > NextCustomerSpawnTime)
            {
                NextCustomerSpawnTime = Random.Range(5, 15);
                CustomerSpawnTimer = 0;
                if (number_of_customers_in_kitchen < 6)
                {
                    GameObject new_customer = Instantiate(CustomerPrefab, CustomerSpawningLocation, transform.rotation);
                    CustomerKitchenQueue.Enqueue(new_customer);
                    Registry.Customers.Add(new_customer);
                    UpdateQueuePositions();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Registry.GameManagerObject.ChangeScene(Constants.PAUSE_MENU);
        }

        HandleApplianceState();
    }
}
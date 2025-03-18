using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    public GameObject CustomerPrefab;

    public Tile IdleCashRegisterTile;
    public Tile ActivatedCashRegisterTile;
    public bool CashRegisterState;

    public Tilemap ApplianceTileMap;

    private Vector2 CustomerSpawningLocation = new Vector2(9.47f, -3.61f);

    private float CustomerSpawnTimer = 0;
    private float NextCustomerSpawnTime = 0;

    private Queue<GameObject> CustomerKitchenQueue = new Queue<GameObject>();

    private bool ReturnToGameToggle = true;

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

    private void AssociateApplianceTilemap()
    {
        Tilemap[] tilemaps = FindObjectsByType<Tilemap>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (Tilemap tilemap in tilemaps)
        {
            if (tilemap.gameObject.name == "MiscAboveTilemap")
            {
                ApplianceTileMap = tilemap.GetComponent<Tilemap>();
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
            Vector2 DestinationPosition = new Vector2(0.5f + spacing * index, -3.61f);
            StartCoroutine(MoveToPosition(obj.GetComponent<Rigidbody2D>(), DestinationPosition));
            index++;
        }
    }

    private IEnumerator MoveToPosition(Rigidbody2D CustomerRigidBody, Vector2 DestinationPosition)
    {
        float duration = 2.0f;
        Vector2 start = CustomerRigidBody.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (Registry.InGameLevel == false)
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

        CashRegisterState = false;

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

                    if (thisCustomer.CustomerRigidBody.position.x == 0.5f)
                    {
                        thisCustomer.Facing = Constants.FACE_UP;
                        thisCustomer.WaitingToBeServed = true;
                        CashRegisterState = true;
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

        if (ApplianceTileMap == null)
        {
            AssociateApplianceTilemap();
        }
        else
        {
            if (CashRegisterState)
            {
                ApplianceTileMap.SwapTile(IdleCashRegisterTile, ActivatedCashRegisterTile);
            }
            else
            {
                ApplianceTileMap.SwapTile(ActivatedCashRegisterTile, IdleCashRegisterTile);
            }
        }
    }
}
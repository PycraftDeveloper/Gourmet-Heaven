using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject CustomerPrefab;

    private Vector2 CustomerSpawningLocation = new Vector2(9.47f, -3.61f);

    private float CustomerSpawnTimer = 0;
    private float NextCustomerSpawnTime = 0;

    private Queue<GameObject> CustomerKitchenQueue = new Queue<GameObject>();

    private void Awake()
    {
        if (Registry.LevelManagerExists == false)
        {
            DontDestroyOnLoad(gameObject);
            Registry.LevelManagerExists = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
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
            CustomerRigidBody.position = Vector2.Lerp(start, DestinationPosition, elapsed / duration);
            elapsed += Registry.GameTimeDelta;
            yield return null;
        }

        CustomerRigidBody.position = DestinationPosition;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Registry.InGameLevel)
        {
            Registry.GameTimeDelta = Time.deltaTime;
        }

        CustomerSpawnTimer += Registry.GameTimeDelta;

        GameObject[] Customers = GameObject.FindGameObjectsWithTag("Customer");

        int number_of_customers_in_kitchen = 0;

        for (int i = 0; i < Customers.Length; i++)
        {
            Customer thisCustomer = Customers[i].GetComponent<Customer>();
            if (Registry.CurrentLocation == Constants.KITCHEN)
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

        if (Registry.CurrentLocation == Constants.KITCHEN)
        {
            if (number_of_customers_in_kitchen == 0 || CustomerSpawnTimer > NextCustomerSpawnTime)
            {
                NextCustomerSpawnTime = Random.Range(5, 15);
                CustomerSpawnTimer = 0;
                if (number_of_customers_in_kitchen < 6)
                {
                    GameObject new_customer = Instantiate(CustomerPrefab, CustomerSpawningLocation, transform.rotation);
                    CustomerKitchenQueue.Enqueue(new_customer);
                    UpdateQueuePositions();
                }
            }
        }
    }
}
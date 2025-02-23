using UnityEngine;

public class LocationManager : MonoBehaviour
{
    public GameObject KitchenLocation;
    public GameObject RestaurantLocation;

    private GameObject KitchenInstance;
    private GameObject RestaurantInstance;

    public string CurrentLocation;
    public bool LocationChanged;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        CurrentLocation = Constants.KITCHEN;
        KitchenInstance = Instantiate(KitchenLocation, transform.position, transform.rotation);
    }

    // Update is called once per frame
    private void Update()
    {
        if (LocationChanged)
        {
            if (CurrentLocation == Constants.KITCHEN)
            {
                Destroy(RestaurantInstance);
                KitchenInstance = Instantiate(KitchenLocation, transform.position, transform.rotation);
            }
            else
            {
                Destroy(KitchenInstance);
                RestaurantInstance = Instantiate(RestaurantLocation, transform.position, transform.rotation);
            }
            LocationChanged = false;
        }
    }
}
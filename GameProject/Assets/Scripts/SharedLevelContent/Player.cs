using UnityEngine;
using UnityEngine.Rendering;

public class texture_exchanger : MonoBehaviour
{
    public Sprite down_texture;
    public Sprite up_texture;
    public Sprite left_texture;
    public Sprite right_texture;

    private SpriteRenderer MySprite;

    public GameObject JoystickInputObject;
    public GameObject LocationManager;

    private PlayerInputCircle JoystickInput;
    private LocationManager LevelLocationManager;

    public int RenderPriority = 1;

    private Vector2 ScreenDimensions;
    private Vector2 SpriteSize;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "RestaurantPortal")
        {
            LevelLocationManager.CurrentLocation = Constants.RESTAURANT;
            LevelLocationManager.LocationChanged = true;
            transform.position = new Vector3(transform.position.x, 4.0f, transform.position.z);
        }
        else if (collider.tag == "KitchenPortal")
        {
            LevelLocationManager.CurrentLocation = Constants.KITCHEN;
            LevelLocationManager.LocationChanged = true;
            transform.position = new Vector3(transform.position.x, -3.9f, transform.position.z);
        }
        Debug.Log(collider.tag);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        MySprite = GetComponent<SpriteRenderer>();
        JoystickInput = JoystickInputObject.GetComponent<PlayerInputCircle>();
        LevelLocationManager = LocationManager.GetComponent<LocationManager>();
        ScreenDimensions = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        SpriteSize = new Vector2(MySprite.bounds.size.x / 2.0f, MySprite.bounds.size.y / 2.0f);
    }

    // Update is called once per frame
    private void Update()
    {
        Vector2 JoystickInputMagnitude = JoystickInput.JoystickOffsetMagnitude;

        float modified_x_position = JoystickInputMagnitude.x * Constants.PLAYER_MOVEMENT_SPEED * Time.deltaTime;
        float modified_y_position = JoystickInputMagnitude.y * Constants.PLAYER_MOVEMENT_SPEED * Time.deltaTime;

        Vector3 proposed_position = new Vector3(transform.position.x + modified_x_position, transform.position.y + modified_y_position, 0);

        proposed_position.x = Mathf.Clamp(proposed_position.x, -ScreenDimensions.x + SpriteSize.x, ScreenDimensions.x - SpriteSize.x);
        proposed_position.y = Mathf.Clamp(proposed_position.y, -ScreenDimensions.y + SpriteSize.y, ScreenDimensions.y - SpriteSize.y);

        transform.position = proposed_position;
        MySprite.sortingOrder = RenderPriority;

        if (Mathf.Abs(JoystickInputMagnitude.x) > Mathf.Abs(JoystickInputMagnitude.y))
        {
            if (JoystickInputMagnitude.x > 0)
            {
                MySprite.sprite = right_texture;
            }
            else if (JoystickInputMagnitude.x < 0)
            {
                MySprite.sprite = left_texture;
            }
        }
        else
        {
            if (JoystickInputMagnitude.y > 0)
            {
                MySprite.sprite = up_texture;
            }
            else if (JoystickInputMagnitude.y < 0)
            {
                MySprite.sprite = down_texture;
            }
        }
    }
}
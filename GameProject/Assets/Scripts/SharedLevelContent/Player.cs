using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class texture_exchanger : MonoBehaviour
{
    public Sprite down_texture;
    public Sprite up_texture;
    public Sprite left_texture;
    public Sprite right_texture;

    private SpriteRenderer MySprite;

    public GameObject JoystickInputObject;

    private PlayerInputCircle JoystickInput;

    public int RenderPriority = 1;

    private Vector2 ScreenDimensions;
    private Vector2 SpriteSize;
    private Rigidbody2D PlayerRigidBody;

    private string CurrentLocation = Constants.KITCHEN;

    private void Awake()
    {
        if (Registry.PlayerExists == false)
        {
            DontDestroyOnLoad(gameObject);
            Registry.PlayerExists = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /*private void OnTriggerEnter2D(Collider2D collider)
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
    }*/

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        MySprite = GetComponent<SpriteRenderer>();
        JoystickInput = JoystickInputObject.GetComponent<PlayerInputCircle>();
        ScreenDimensions = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        SpriteSize = new Vector2(MySprite.bounds.size.x / 2.0f, MySprite.bounds.size.y / 2.0f);
        PlayerRigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 JoystickInputMagnitude = JoystickInput.JoystickOffsetMagnitude;

        float modified_x_position = JoystickInputMagnitude.x * Constants.PLAYER_MOVEMENT_SPEED * Time.deltaTime;
        float modified_y_position = JoystickInputMagnitude.y * Constants.PLAYER_MOVEMENT_SPEED * Time.deltaTime;

        Vector3 proposed_position = new Vector3(PlayerRigidBody.position.x + modified_x_position, PlayerRigidBody.position.y + modified_y_position, 0);

        float minimum_y = -ScreenDimensions.y + SpriteSize.y;
        float maximum_y = ScreenDimensions.y - SpriteSize.y;

        proposed_position.x = Mathf.Clamp(proposed_position.x, -ScreenDimensions.x + SpriteSize.x, ScreenDimensions.x - SpriteSize.x);
        proposed_position.y = Mathf.Clamp(proposed_position.y, minimum_y, maximum_y);

        if (proposed_position.y == minimum_y && CurrentLocation == Constants.KITCHEN)
        {
            SceneManager.LoadScene("Restaurant");

            CurrentLocation = Constants.RESTAURANT;

            proposed_position.y = maximum_y - 0.01f;
        }
        else if (proposed_position.y == maximum_y && CurrentLocation == Constants.RESTAURANT)
        {
            SceneManager.LoadScene("Kitchen");

            CurrentLocation = Constants.KITCHEN;

            proposed_position.y = minimum_y + 0.01f;
        }

        PlayerRigidBody.MovePosition(proposed_position);
        //transform.position = proposed_position;
        MySprite.sortingOrder = RenderPriority;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector2 JoystickInputMagnitude = JoystickInput.JoystickOffsetMagnitude;

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
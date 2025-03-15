using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Sprite down_texture;
    public Sprite up_texture;
    public Sprite side_texture;

    public Sprite[] IdleApplianceSprites = new Sprite[4];
    public Sprite[] ActivatedApplianceSprites = new Sprite[4];
    public bool[] SpriteStates = new bool[4];

    private List<string> PlacedMeals = new List<string>();

    private SpriteRenderer PlayerSprite;

    public GameObject JoystickInputObject;

    private PlayerInputCircle JoystickInput;

    public int RenderPriority = 1;

    private Vector2 ScreenDimensions;
    private Vector2 SpriteSize;
    private Rigidbody2D PlayerRigidBody;

    public Animator PlayerAnimator;

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

    private void OnTriggerStay2D(Collider2D collider)
    {
        /*if (collider.tag == "RestaurantPortal")
        {
            Registry.CurrentLocation = Constants.RESTAURANT;
            PlayerRigidBody.MovePosition(new Vector3(transform.position.x, 4.0f, transform.position.z));
            SceneManager.LoadScene("Restaurant");
        }
        else if (collider.tag == "KitchenPortal")
        {
            Registry.CurrentLocation = Constants.KITCHEN;
            PlayerRigidBody.MovePosition(new Vector3(transform.position.x, -3.9f, transform.position.z));
            SceneManager.LoadScene("Kitchen");
        }*/
        Debug.Log(collider.name);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        PlayerSprite = GetComponent<SpriteRenderer>();
        JoystickInput = JoystickInputObject.GetComponent<PlayerInputCircle>();
        ScreenDimensions = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        SpriteSize = new Vector2(PlayerSprite.bounds.size.x / 2.0f, PlayerSprite.bounds.size.y / 2.0f);
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

        if (proposed_position.y == minimum_y && Registry.CurrentLocation == Constants.KITCHEN)
        {
            SceneManager.LoadScene("Restaurant");

            Registry.CurrentLocation = Constants.RESTAURANT;

            proposed_position.y = maximum_y - 0.01f;
        }
        else if (proposed_position.y == maximum_y && Registry.CurrentLocation == Constants.RESTAURANT)
        {
            SceneManager.LoadScene("Kitchen");

            Registry.CurrentLocation = Constants.KITCHEN;

            proposed_position.y = minimum_y + 0.01f;
        }

        PlayerRigidBody.MovePosition(proposed_position);
        //transform.position = proposed_position;
        PlayerSprite.sortingOrder = RenderPriority;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector2 JoystickInputMagnitude = JoystickInput.JoystickOffsetMagnitude;

        if (Mathf.Min(JoystickInputMagnitude.x, JoystickInputMagnitude.y) == 0)
        {
            PlayerAnimator.SetInteger("playerState", 0);
        }

        if (Mathf.Abs(JoystickInputMagnitude.x) > Mathf.Abs(JoystickInputMagnitude.y))
        {
            if (JoystickInputMagnitude.x != 0)
            {
                PlayerAnimator.SetInteger("playerState", 3);
                if (JoystickInputMagnitude.x > 0)
                {
                    Vector3 scale = PlayerSprite.transform.localScale;
                    if (scale.x > 0)
                    {
                        scale.x *= -1;
                        PlayerSprite.transform.localScale = scale;
                    }
                }
                else
                {
                    Vector3 scale = PlayerSprite.transform.localScale;
                    if (scale.x < 0)
                    {
                        scale.x *= -1;
                        PlayerSprite.transform.localScale = scale;
                    }
                }
                PlayerSprite.sprite = side_texture;
            }
        }
        else
        {
            if (JoystickInputMagnitude.y > 0)
            {
                PlayerAnimator.SetInteger("playerState", 1);
                PlayerSprite.sprite = up_texture;
            }
            else if (JoystickInputMagnitude.y < 0)
            {
                PlayerAnimator.SetInteger("playerState", 2);
                PlayerSprite.sprite = down_texture;
            }
        }
    }
}
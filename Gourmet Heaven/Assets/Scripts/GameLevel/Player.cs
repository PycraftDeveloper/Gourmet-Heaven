using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject[] AppliancePopUpMessages = new GameObject[6]; // Store the different pop-up game objects the player clicks to perform tasks in the game.
    public Sprite[] TillPointPopUpSprites = new Sprite[4]; // Store the different sprites for the till-point pop-up messages, so it is easier to see what the customer wants at the till point.
    public Sprite[] HoldingMealPopUpSprites = new Sprite[4]; // Store the different thought bubbles that show that meal the player is holding.

    private Renderer PlayerSprite;
    public GameObject JoystickInputObject;
    private PlayerInputCircle JoystickInput; // Keep a reference to the joystick input so we can use it to move the player.
    private Rigidbody2D PlayerRigidBody;
    public Animator PlayerAnimator;
    public GameObject PlayerHoldingPopUp;

    public int RenderPriority = 1; // The render priority of the player sprite, used to create the illusion of depth when interacting with customers in the restaurant.

    private Vector2 ScreenDimensions; // Store the screen dimensions to ensure the player can't move off the screen. This is less of an issue now with collisions in both scenes

    // but still has a purpose.
    private Vector2 SpriteSize; // Store the size of the player.

    public bool SceneChanged = false;

    public string HoldingMeal = Constants.NOT_HOLDING_MEAL; // Keep track of any meal the player may be holding. Can hold ONLY ONE meal.

    public void SetAnimationState(int state)
    {
        if (HoldingMeal != Constants.NOT_HOLDING_MEAL) // Consider holding meal animations by adding 4 to the animation state.
        {
            state += 4;
        }
        PlayerAnimator.SetInteger("playerState", state);
    }

    private void Awake() // This object persists across scene changes and is managed by the GameManager.
    {
        Registry.PlayerObject = this;
    }

    private void OnTriggerStay2D(Collider2D Collider)
    {
        if (Collider.name == "CashRegister_AreaDetector" && AppliancePopUpMessages[0] != null)
        {
            if (Registry.LevelManagerObject.CacheRegister.GetState())
            {
                if (!AppliancePopUpMessages[0].activeSelf) // Show the till-point interact button when in area hit-box area.
                {
                    ForegroundCustomer CustomerAtTill = Registry.LevelManagerObject.CustomerKitchenQueue.Peek();
                    // Change the button icon to match what the customer wants.
                    switch (CustomerAtTill.Meal)
                    {
                        case Constants.PHO:
                            AppliancePopUpMessages[0].GetComponent<Image>().sprite = TillPointPopUpSprites[0];
                            break;

                        case Constants.SUSHI:
                            AppliancePopUpMessages[0].GetComponent<Image>().sprite = TillPointPopUpSprites[1];
                            break;

                        case Constants.BAO_BUNS:
                            AppliancePopUpMessages[0].GetComponent<Image>().sprite = TillPointPopUpSprites[2];
                            break;

                        case Constants.MANGO_STICKY_RICE:
                            AppliancePopUpMessages[0].GetComponent<Image>().sprite = TillPointPopUpSprites[3];
                            break;
                    }
                }
                AppliancePopUpMessages[0].SetActive(true); // Show the till point.
            }
            else
            {
                AppliancePopUpMessages[0].SetActive(false); // Hide the till point.
            }
        }
        else if (Collider.name == "ChoppingBoard_AreaDetector" && AppliancePopUpMessages[1] != null) // If collided with the chopping board area hit box
        {
            if (Registry.LevelManagerObject.ChoppingBoard.GetState()) // show or hide the button to go to the mango sticky rice mini-game
            {
                AppliancePopUpMessages[1].SetActive(true);
            }
            else
            {
                AppliancePopUpMessages[1].SetActive(false);
            }
        }
        else if (Collider.name == "CookingPot_AreaDetector" && AppliancePopUpMessages[2] != null) // If collided with the cooking pot area hit box
        {
            if (Registry.LevelManagerObject.CookingPot.GetState()) // show or hide the button to go to the bao buns mini-game
            {
                AppliancePopUpMessages[2].SetActive(true);
            }
            else
            {
                AppliancePopUpMessages[2].SetActive(false);
            }
        }
        else if (Collider.name == "PhoBowl_AreaDetector" && AppliancePopUpMessages[3] != null) // If collided with the Pho bowl area hit box
        {
            if (Registry.LevelManagerObject.PhoBowl.GetState()) // show or hide the button to go to the Pho mini-game
            {
                AppliancePopUpMessages[3].SetActive(true);
            }
            else
            {
                AppliancePopUpMessages[3].SetActive(false);
            }
        }
        else if (Collider.name == "SushiRollingMat_AreaDetector" && AppliancePopUpMessages[4] != null) // If collided with the sushi area hit box
        {
            if (Registry.LevelManagerObject.SushiRollingMat.GetState()) // show or hide the button to go to the sushi mini-game
            {
                AppliancePopUpMessages[4].SetActive(true);
            }
            else
            {
                AppliancePopUpMessages[4].SetActive(false);
            }
        }

        if (Collider.CompareTag("Customer")) // If collided with a customer in the restaurant
        {
            CustomerCore customer = Collider.gameObject.GetComponent<CustomerCore>();

            float playerY = PlayerRigidBody.position.y;
            float playerHeight = PlayerSprite.bounds.size.y;
            float customerY = customer.CurrentPosition.y;
            float customerHeight = Collider.bounds.size.y;

            if (customerY + customerHeight > playerY && customerY > playerY) // determine if the player should be rendered on top of the customer
            {
                PlayerSprite.sortingLayerName = "Player Upper";
            }
            else if (customerY < playerY + playerHeight) // determine if the player should be rendered below the customer.
            {
                PlayerSprite.sortingLayerName = "Player & NPCs";
            }
        }
    }

    private void OnTriggerExit2D(Collider2D Collider)
    {
        // When no longer in collision with an area hit box, hide the assoctaed button.
        if (Collider.name == "CashRegister_AreaDetector" && AppliancePopUpMessages[0] != null)
        {
            AppliancePopUpMessages[0].SetActive(false);
            return;
        }
        if (Collider.name == "ChoppingBoard_AreaDetector" && AppliancePopUpMessages[1] != null)
        {
            AppliancePopUpMessages[1].SetActive(false);
            return;
        }
        if (Collider.name == "CookingPot_AreaDetector" && AppliancePopUpMessages[2] != null)
        {
            AppliancePopUpMessages[2].SetActive(false);
            return;
        }
        if (Collider.name == "PhoBowl_AreaDetector" && AppliancePopUpMessages[3] != null)
        {
            AppliancePopUpMessages[3].SetActive(false);
            return;
        }
        if (Collider.name == "SushiRollingMat_AreaDetector" && AppliancePopUpMessages[4] != null)
        {
            AppliancePopUpMessages[4].SetActive(false);
            return;
        }
        if (Collider.CompareTag("Customer"))
        {
            PlayerSprite.sortingLayerName = "Player & NPCs"; // If the player is no longer colliding with a customer in the restaurant, reset the player's layer.
            return;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        PlayerSprite = GetComponent<Renderer>();
        JoystickInput = JoystickInputObject.GetComponent<PlayerInputCircle>();
        ScreenDimensions = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize);
        SpriteSize = new Vector2(PlayerSprite.bounds.size.x / 2.0f, PlayerSprite.bounds.size.y / 2.0f);
        PlayerRigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Get the input from the joystick, and convert that into a movement vector for the player.
        Vector2 JoystickInputMagnitude = JoystickInput.JoystickOffsetMagnitude;

        float modified_x_position = JoystickInputMagnitude.x * Constants.PLAYER_MOVEMENT_SPEED * Time.deltaTime;
        float modified_y_position = JoystickInputMagnitude.y * Constants.PLAYER_MOVEMENT_SPEED * Time.deltaTime;

        Vector3 proposed_position = new Vector3(PlayerRigidBody.position.x + modified_x_position, PlayerRigidBody.position.y + modified_y_position, 0);

        float minimum_y = (-ScreenDimensions.y * 2) + SpriteSize.y;
        float maximum_y = ScreenDimensions.y - SpriteSize.y;

        PlayerRigidBody.MovePosition(proposed_position);
        //transform.position = proposed_position;
        PlayerSprite.sortingOrder = RenderPriority;
    }

    // Update is called once per frame
    private void Update()
    {
        if (SceneChanged)
        {
            SceneChanged = false;
            Button[] SceneButtons = FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None); // get all the buttons in the scene, to re-associate the
            // appliance pop-up messages with the buttons in the scene.
            AppliancePopUpMessages = new GameObject[6];

            foreach (Button SceneButton in SceneButtons)
            {
                if (SceneButton.name == "CachierPopUp")
                {
                    AppliancePopUpMessages[0] = SceneButton.gameObject;
                }
                else if (SceneButton.name == "ChoppingBoardPopUp")
                {
                    AppliancePopUpMessages[1] = SceneButton.gameObject;
                }
                else if (SceneButton.name == "CookingPotPopUp")
                {
                    AppliancePopUpMessages[2] = SceneButton.gameObject;
                }
                else if (SceneButton.name == "PhoBowlPopUp")
                {
                    AppliancePopUpMessages[3] = SceneButton.gameObject;
                }
                else if (SceneButton.name == "RollingMatPopUp")
                {
                    AppliancePopUpMessages[4] = SceneButton.gameObject;
                }
                else if (SceneButton.name == "BinPopUp")
                {
                    AppliancePopUpMessages[5] = SceneButton.gameObject;
                }
            }
        }

        Vector2 JoystickInputMagnitude = JoystickInput.JoystickOffsetMagnitude;

        if (Mathf.Min(JoystickInputMagnitude.x, JoystickInputMagnitude.y) == 0) // if the player isn't moving in the left or right axis
        {
            SetAnimationState(Constants.PLAYER_IDLE_ANIMATION); // play the idle animation
        }

        if (Mathf.Abs(JoystickInputMagnitude.x) > Mathf.Abs(JoystickInputMagnitude.y)) // if the player is moving more in the x axis
        {
            if (JoystickInputMagnitude.x != 0)
            {
                SetAnimationState(Constants.PLAYER_WALK_SIDE_ANIMATION); // Play the side walk animation, and make the player face in the right direction for travel.
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
            }
        }
        else // if moving more in the y axis
        {
            if (JoystickInputMagnitude.y > 0) // if walking up to the top of the screen
            {
                SetAnimationState(Constants.PLAYER_WALK_UP_ANIMATION); // Play the up walk animation.
            }
            else if (JoystickInputMagnitude.y < 0) // if walking down to the bottom of the screen
            {
                SetAnimationState(Constants.PLAYER_WALK_DOWN_ANIMATION); // Play the walk down animation.
            }
        }

        if (AppliancePopUpMessages[5] != null)
        {
            AppliancePopUpMessages[5].SetActive(HoldingMeal != Constants.NOT_HOLDING_MEAL); // show/hide the bin pop-up button depending on if the player is holding a meal.
        }

        PlayerHoldingPopUp.SetActive(HoldingMeal != Constants.NOT_HOLDING_MEAL); // show what meal the player is holding when the player is holding a meal.

        // Update the player holding pop-up sprite depending on what meal the player is holding.
        if (HoldingMeal != Constants.NOT_HOLDING_MEAL)
        {
            switch (HoldingMeal)
            {
                case Constants.PHO:
                    PlayerHoldingPopUp.GetComponent<SpriteRenderer>().sprite = HoldingMealPopUpSprites[0];
                    break;

                case Constants.SUSHI:
                    PlayerHoldingPopUp.GetComponent<SpriteRenderer>().sprite = HoldingMealPopUpSprites[1];
                    break;

                case Constants.BAO_BUNS:
                    PlayerHoldingPopUp.GetComponent<SpriteRenderer>().sprite = HoldingMealPopUpSprites[2];
                    break;

                case Constants.MANGO_STICKY_RICE:
                    PlayerHoldingPopUp.GetComponent<SpriteRenderer>().sprite = HoldingMealPopUpSprites[3];
                    break;
            }
        }
    }
}
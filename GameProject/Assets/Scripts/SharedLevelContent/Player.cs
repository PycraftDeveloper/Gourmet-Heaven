using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject[] AppliancePopUpMessages = new GameObject[6];
    public Sprite[] TillPointPopUpSprites = new Sprite[4];
    public Sprite[] HoldingMealPopUpSprites = new Sprite[4];

    private Renderer PlayerSprite;
    public GameObject JoystickInputObject;
    private PlayerInputCircle JoystickInput;
    private Rigidbody2D PlayerRigidBody;
    public Animator PlayerAnimator;
    public GameObject PlayerHoldingPopUp;

    private List<string> PlacedMeals = new List<string>();

    public int RenderPriority = 1;

    private Vector2 ScreenDimensions;
    private Vector2 SpriteSize;

    public bool SceneChanged = false;

    public string HoldingMeal = Constants.NOT_HOLDING_MEAL;

    public void SetAnimationState(int state)
    {
        if (HoldingMeal != Constants.NOT_HOLDING_MEAL)
        {
            state += 4;
        }
        PlayerAnimator.SetInteger("playerState", state);
    }

    private void Awake()
    {
        if (Registry.PlayerObject == null)
        {
            DontDestroyOnLoad(gameObject);
            Registry.PlayerObject = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D Collider)
    {
        if (Registry.CurrentSceneName == Constants.KITCHEN)
        {
            if (Collider.name == "CashRegister_AreaDetector" && AppliancePopUpMessages[0] != null)
            {
                if (Registry.LevelManagerObject.CacheRegister.GetState())
                {
                    if (!AppliancePopUpMessages[0].activeSelf)
                    {
                        GameObject CustomerObjectAtTill = Registry.LevelManagerObject.CustomerKitchenQueue.Peek();
                        Customer CustomerAtTill = CustomerObjectAtTill.GetComponent<Customer>();
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
                    AppliancePopUpMessages[0].SetActive(true);
                }
                else
                {
                    AppliancePopUpMessages[0].SetActive(false);
                }
            }
            else if (Collider.name == "ChoppingBoard_AreaDetector" && AppliancePopUpMessages[1] != null)
            {
                    if (Registry.LevelManagerObject.ChoppingBoard.GetState())
                    {
                        AppliancePopUpMessages[1].SetActive(true);
                    }
                    else
                    {
                        AppliancePopUpMessages[1].SetActive(false);
                    }
            }
            else if (Collider.name == "CookingPot_AreaDetector" && AppliancePopUpMessages[2] != null)
            {
                if (Registry.LevelManagerObject.CookingPot.GetState())
                {
                    AppliancePopUpMessages[2].SetActive(true);
                }
                else
                {
                    AppliancePopUpMessages[2].SetActive(false);
                }
            }
            else if (Collider.name == "PhoBowl_AreaDetector" && AppliancePopUpMessages[3] != null)
            {
                if (Registry.LevelManagerObject.PhoBowl.GetState())
                {
                    AppliancePopUpMessages[3].SetActive(true);
                }
                else
                {
                    AppliancePopUpMessages[3].SetActive(false);
                }
            }
            else if (Collider.name == "SushiRollingMat_AreaDetector" && AppliancePopUpMessages[4] != null)
            {
                if (Registry.LevelManagerObject.SushiRollingMat.GetState())
                {
                    AppliancePopUpMessages[4].SetActive(true);
                }
                else
                {
                    AppliancePopUpMessages[4].SetActive(false);
                }
            }
        }
        else if (Collider.CompareTag("Customer") && Registry.CurrentSceneName == Constants.RESTAURANT)
        {
            CustomerCore customer = Collider.gameObject.GetComponent<CustomerCore>();

            float playerY = PlayerRigidBody.position.y;
            float playerHeight = PlayerSprite.bounds.size.y;
            float customerY = customer.CurrentPosition.y;
            float customerHeight = Collider.bounds.size.y;

            if (customerY + customerHeight > playerY && customerY > playerY) // top
            {
                PlayerSprite.sortingLayerName = "Player Upper";
            }
            else if (customerY < playerY + playerHeight) // bottom
            {
                PlayerSprite.sortingLayerName = "Player & NPCs";
            }
        }
    }

    private void OnTriggerExit2D(Collider2D Collider)
    {
        if (Registry.CurrentSceneName == Constants.KITCHEN)
        {
            if (Collider.name == "CashRegister_AreaDetector" && AppliancePopUpMessages[0] != null)
            {
                AppliancePopUpMessages[0].SetActive(false);
            }
            else if (Collider.name == "ChoppingBoard_AreaDetector" && AppliancePopUpMessages[1] != null)
            {
                AppliancePopUpMessages[1].SetActive(false);
            }
            else if (Collider.name == "CookingPot_AreaDetector" && AppliancePopUpMessages[2] != null)
            {
                AppliancePopUpMessages[2].SetActive(false);
            }
            else if (Collider.name == "PhoBowl_AreaDetector" && AppliancePopUpMessages[3] != null)
            {
                AppliancePopUpMessages[3].SetActive(false);
            }
            else if (Collider.name == "SushiRollingMat_AreaDetector" && AppliancePopUpMessages[4] != null)
            {
                AppliancePopUpMessages[4].SetActive(false);
            }
        }
        else if (Collider.CompareTag("Customer") && Registry.CurrentSceneName == Constants.RESTAURANT)
        {
            PlayerSprite.sortingLayerName = "Player & NPCs";
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
        Vector2 JoystickInputMagnitude = JoystickInput.JoystickOffsetMagnitude;

        float modified_x_position = JoystickInputMagnitude.x * Constants.PLAYER_MOVEMENT_SPEED * Time.deltaTime;
        float modified_y_position = JoystickInputMagnitude.y * Constants.PLAYER_MOVEMENT_SPEED * Time.deltaTime;

        Vector3 proposed_position = new Vector3(PlayerRigidBody.position.x + modified_x_position, PlayerRigidBody.position.y + modified_y_position, 0);

        float minimum_y = -ScreenDimensions.y + SpriteSize.y;
        float maximum_y = ScreenDimensions.y - SpriteSize.y;

        proposed_position.x = Mathf.Clamp(proposed_position.x, -ScreenDimensions.x + SpriteSize.x, ScreenDimensions.x - SpriteSize.x);
        proposed_position.y = Mathf.Clamp(proposed_position.y, minimum_y, maximum_y);

        if (proposed_position.x > -4.46 && proposed_position.x < -2.56)
        {
            if (proposed_position.y == minimum_y && Registry.CurrentSceneName == Constants.KITCHEN)
            {
                Registry.GameManagerObject.ChangeScene(Constants.RESTAURANT);

                proposed_position.y = maximum_y - 0.01f;
            }
            else if (proposed_position.y == maximum_y && Registry.CurrentSceneName == Constants.RESTAURANT)
            {
                Registry.GameManagerObject.ChangeScene(Constants.KITCHEN);

                proposed_position.y = minimum_y + 0.01f;
            }
        }
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
            Button[] SceneButtons = FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);
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

        if (Mathf.Min(JoystickInputMagnitude.x, JoystickInputMagnitude.y) == 0)
        {
            SetAnimationState(Constants.PLAYER_IDLE_ANIMATION);
        }

        if (Mathf.Abs(JoystickInputMagnitude.x) > Mathf.Abs(JoystickInputMagnitude.y))
        {
            if (JoystickInputMagnitude.x != 0)
            {
                SetAnimationState(Constants.PLAYER_WALK_SIDE_ANIMATION);
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
        else
        {
            if (JoystickInputMagnitude.y > 0)
            {
                SetAnimationState(Constants.PLAYER_WALK_UP_ANIMATION);
            }
            else if (JoystickInputMagnitude.y < 0)
            {
                SetAnimationState(Constants.PLAYER_WALK_DOWN_ANIMATION);
            }
        }

        if (Registry.CurrentSceneName == Constants.KITCHEN)
        {
            if (AppliancePopUpMessages[5] != null)
            {
                AppliancePopUpMessages[5].SetActive(HoldingMeal != Constants.NOT_HOLDING_MEAL);
            }
        }

        PlayerHoldingPopUp.SetActive(HoldingMeal != Constants.NOT_HOLDING_MEAL);

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
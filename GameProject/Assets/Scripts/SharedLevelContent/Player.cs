using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Sprite down_texture;
    public Sprite up_texture;
    public Sprite side_texture;

    public Sprite[] IdleApplianceSprites = new Sprite[4];
    public Sprite[] ActivatedApplianceSprites = new Sprite[4];

    private List<string> PlacedMeals = new List<string>();

    private SpriteRenderer PlayerSprite;

    public GameObject JoystickInputObject;
    public GameObject[] AppliancePopUpMessages = new GameObject[5];

    private PlayerInputCircle JoystickInput;

    public int RenderPriority = 1;

    private Vector2 ScreenDimensions;
    private Vector2 SpriteSize;
    private Rigidbody2D PlayerRigidBody;

    public Animator PlayerAnimator;

    public bool SceneChanged = false;

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
            if (Collider.name == "CashRegister_AreaDetector")
            {
                if (Registry.LevelManagerObject.CacheRegister.GetState())
                {
                    AppliancePopUpMessages[0].SetActive(true);
                }
                else
                {
                    AppliancePopUpMessages[0].SetActive(false);
                }
            }
            else if (Collider.name == "ChoppingBoard_AreaDetector")
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
            else if (Collider.name == "CookingPot_AreaDetector")
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
            else if (Collider.name == "PhoBowl_AreaDetector")
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
            else if (Collider.name == "SushiRollingMat_AreaDetector")
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
    }

    private void OnTriggerExit2D(Collider2D Collider)
    {
        if (Registry.CurrentSceneName == Constants.KITCHEN)
        {
            if (Collider.name == "CashRegister_AreaDetector")
            {
                AppliancePopUpMessages[0].SetActive(false);
            }
            else if (Collider.name == "ChoppingBoard_AreaDetector")
            {
                AppliancePopUpMessages[1].SetActive(false);
            }
            else if (Collider.name == "CookingPot_AreaDetector")
            {
                AppliancePopUpMessages[2].SetActive(false);
            }
            else if (Collider.name == "PhoBowl_AreaDetector")
            {
                AppliancePopUpMessages[3].SetActive(false);
            }
            else if (Collider.name == "SushiRollingMat_AreaDetector")
            {
                AppliancePopUpMessages[4].SetActive(false);
            }
        }
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
        if (SceneChanged)
        {
            SceneChanged = false;
            Button[] SceneButtons = FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            AppliancePopUpMessages = new GameObject[5];

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
            }
        }
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
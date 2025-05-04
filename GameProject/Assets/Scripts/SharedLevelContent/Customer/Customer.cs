using System.Collections;
using UnityEngine;

public class Customer : CustomerCore
{
    public IEnumerator CustomerCoroutine = null;
    private MonoBehaviour GameManagerMono;

    public string Meal;
    public string CustomerCoroutineDescription = Constants.NO_COROUTINE;

    public Vector2 CurrentVelocity = Vector2.zero;

    private int CustomerAnimationState;

    public bool MealPlaced = false;

    public GameObject[] OrderPopUpMessages = new GameObject[4];
    public GameObject InstantiatedOrderPopUpMessage;

    public IEnumerator MoveInQueue(Vector2 DestinationPosition)
    {
        Vector2 TargetPosition = CurrentPosition;

        if (TargetPosition.x != DestinationPosition.x)
        {
            SetAnimationState(Constants.CUSTOMER_WALK_SIDE_ANIMATION);

            while (CurrentPosition.x - (Constants.CUSTOMER_MOVEMENT_SPEED * Time.deltaTime) > DestinationPosition.x && Registry.InGameLevel == true && gameObject != null)
            {
                CurrentPosition.x -= Constants.CUSTOMER_MOVEMENT_SPEED * Time.deltaTime;
                yield return null;
            }

            if (CurrentPosition.x - (Constants.CUSTOMER_MOVEMENT_SPEED * Time.deltaTime) < DestinationPosition.x)
            {
                CurrentPosition = DestinationPosition;
            }

            SetAnimationState(Constants.CUSTOMER_IDLE_SIDE_ANIMATION);
        }
    }

    public IEnumerator MoveIntoRestaurant() // Used to give the illusion of customers walking out the bottom of the Kitchen, into the restaurant.
    {
        SetAnimationState(Constants.CUSTOMER_WALK_DOWN_ANIMATION); // Set the animation to walking.
        while (CurrentLocation == Constants.KITCHEN && gameObject != null) // Whilst the player is in the same scene, and the customer hasn't been deleted
                                                                           // which is mainly used for stopping execution through the editor and handling errors there.
        {
            CurrentPosition = new Vector2(
                CurrentPosition.x,
                CurrentPosition.y - Constants.CUSTOMER_MOVEMENT_SPEED * Time.deltaTime); // move the customer down the scene.

            if (CurrentPosition.y < -6.31 || Registry.InGameLevel == false) // When the customer is no longer visible in the
                                                                            // kitchen, break out the while loop s it can be set up in the restaurant.
            {
                CurrentLocation = Constants.RESTAURANT;
            }
            yield return null;
        }
        PlaceIntoRestaurant(); // Set the customer up in the restaurant.
    }

    private void PlaceIntoRestaurant() // Similar to before, this is used to place customers in the restaurant scene.
                                       // HOWEVER, this is for customers the player can interact with, NOT for background customers.
    {
        bool Seated = false;
        while (!Seated)
        {
            int PositionIndex = Random.Range(0, 8);
            GameObject[] CustomerTableArrangement = Registry.LevelManagerObject.CustomerTableArrangement;
            if (CustomerTableArrangement[PositionIndex] == null || CustomerTableArrangement[PositionIndex].GetComponent<BackgroundCustomer>())
            {
                if (CustomerTableArrangement[PositionIndex] != null && CustomerTableArrangement[PositionIndex].GetComponent<BackgroundCustomer>())
                {
                    Registry.Customers.Remove(CustomerTableArrangement[PositionIndex]);
                    Destroy(CustomerTableArrangement[PositionIndex]);
                    CustomerTableArrangement[PositionIndex] = null;
                }
                if (PositionIndex % 2 == 0)
                {
                    Vector3 CustomerScale = transform.localScale;
                    CustomerScale.x *= -1;
                    transform.localScale = CustomerScale;
                }
                SetAnimationState(Constants.CUSTOMER_IDLE_SIT_ANIMATION); // Ensure that when placed into the restaurant, the customer is in the sitting animation
                SetupCustomerCoreForRestaurant(PositionIndex);
                Seated = true;
            }
        }
        if (Registry.CurrentSceneName != Constants.RESTAURANT) // In this situation, we want to hide the customer from the kitchen scene (where the player is) until
                                                               // the player goes into the restaurant scene.
        {
            gameObject.SetActive(false);
        }
    }

    private void HandleCustomerTouched(Vector2 TouchPosition)
    {
        bool CorrectMealServed = false;
        Vector2 WorldPosition = Camera.main.ScreenToWorldPoint(TouchPosition);
        Collider2D RaycastHit = Physics2D.OverlapPoint(WorldPosition);

        if (RaycastHit != null && RaycastHit.transform == transform)
        {
            Registry.MaxScore += 100;
            if (Registry.PlayerObject.HoldingMeal == Meal)
            {
                CorrectMealServed = true;
                // start - this section of code was worked on by Joshua Cossar (v)
                int CustomerSound = Random.Range(0, 3);
                if (CustomerSound == 0)
                {
                    Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.CustomerFinish1);
                }
                if (CustomerSound == 1)
                {
                    Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.CustomerFinish2);
                }
                if (CustomerSound == 2)
                {
                    Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.CustomerFinish3);
                }
                // end - this section of code was worked on by Joshua Cossar
                Registry.PlayerScore += 100;
            }

            Registry.PlayerObject.HoldingMeal = Constants.NOT_HOLDING_MEAL;

            Destroy(InstantiatedOrderPopUpMessage);
            if (Registry.LevelNumber != Constants.LEVEL_ONE)
            {
                int[] CustomerSecondMealRange = Constants.CUSTOMER_LEVEL_TWO_SECOND_MEAL_RANGE;
                bool DoSecondMeal = Random.Range(CustomerSecondMealRange[0], CustomerSecondMealRange[1]) == 0;

                if (DoSecondMeal && CorrectMealServed)
                {
                    GenerateMeal();
                    Patience = Random.Range(
                Constants.CUSTOMER_MIN_PATIENCE[Registry.LevelNumber],
                Constants.CUSTOMER_MAX_PATIENCE[Registry.LevelNumber]);
                    InitialPatience = Patience;
                }
                else
                {
                    Meal = "";
                    Patience = 0;
                    InitialPatience = 0;
                    PatienceMeter.SetActive(false);
                }
            }
            else
            {
                Meal = "";
                Patience = 0;
                InitialPatience = 0;
                PatienceMeter.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (CurrentLocation == Constants.RESTAURANT && InstantiatedOrderPopUpMessage != null && Registry.PlayerObject.HoldingMeal != Constants.NOT_HOLDING_MEAL)
        {
            if (Input.touchCount > 0) // if touch input is used
            {
                UnityEngine.Touch touch = Input.GetTouch(0); // get the first finger (only guaranteed input)

                HandleCustomerTouched(touch.position); // handle position adjustments, using finger position on-screen.
            }
            else if (Input.GetMouseButton(0)) // if mouse click is used
            {
                HandleCustomerTouched(Input.mousePosition); // handle position adjustments, using mouse position.
            }
        }

        if (DeSpawn && InstantiatedOrderPopUpMessage != null)
        {
            Destroy(InstantiatedOrderPopUpMessage);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        GameManagerMono = Registry.GameManagerObject.GetComponent<MonoBehaviour>();

        GenerateMeal();
        SetAnimationState(Constants.CUSTOMER_WALK_SIDE_ANIMATION);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (CurrentLocation == Constants.RESTAURANT && InstantiatedOrderPopUpMessage == null && DeSpawn == false)
            {
                //Vector2 PopUpPosition = CurrentPosition;
                //PopUpPosition.y += 0.3f + _Renderer.bounds.size.y / 2.0f;

                if (Meal == Constants.BAO_BUNS)
                {
                    InstantiatedOrderPopUpMessage = Instantiate(OrderPopUpMessages[0]);
                }
                else if (Meal == Constants.MANGO_STICKY_RICE)
                {
                    InstantiatedOrderPopUpMessage = Instantiate(OrderPopUpMessages[1]);
                }
                else if (Meal == Constants.PHO)
                {
                    InstantiatedOrderPopUpMessage = Instantiate(OrderPopUpMessages[2]);
                }
                else
                {
                    InstantiatedOrderPopUpMessage = Instantiate(OrderPopUpMessages[3]);
                }

                if (CustomerTablePosition % 2 == 1 && InstantiatedOrderPopUpMessage != null)
                {
                    Vector2 PopUpScale = InstantiatedOrderPopUpMessage.transform.localScale;
                    PopUpScale.x *= -1;
                    InstantiatedOrderPopUpMessage.transform.localScale = PopUpScale;
                }

                InstantiatedOrderPopUpMessage.transform.SetParent(transform, true);

                Vector2 Position = new Vector2(-0.28f, 1.24f);
                InstantiatedOrderPopUpMessage.transform.localPosition = Position;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (InstantiatedOrderPopUpMessage != null)
        {
            Destroy(InstantiatedOrderPopUpMessage);
        }
    }

    public void SetAnimationState(int StateNumber)
    {
        if (_Animator != null)
        {
            CustomerAnimationState = StateNumber;
            _Animator.SetInteger("customerState", CustomerAnimationState);
        }
    }

    public void SetCoroutine(string description)
    {
        if (description == Constants.NO_COROUTINE)
        {
            GameManagerMono.StopCoroutine(CustomerCoroutine);
            CustomerCoroutine = null;
            CustomerCoroutineDescription = Constants.NO_COROUTINE;
        }
    }

    public void SetCoroutine(IEnumerator coroutine, string description)
    {
        if (CustomerCoroutine == null)
        {
            CustomerCoroutine = coroutine;
            CustomerCoroutineDescription = description;
        }
        else if (description == Constants.MOVE_INTO_RESTAURANT && CustomerCoroutineDescription == Constants.MOVE_IN_QUEUE)
        {
            GameManagerMono.StopCoroutine(CustomerCoroutine);
            CustomerCoroutine = coroutine;
            CustomerCoroutineDescription = description;
        }
        else if (description == Constants.MOVE_IN_QUEUE && CustomerCoroutineDescription == Constants.MOVE_IN_QUEUE)
        {
            GameManagerMono.StopCoroutine(CustomerCoroutine);
            CustomerCoroutine = coroutine;
        }
        if (_Renderer == null)
        {
            _Renderer = GetComponent<Renderer>();
        }
        GameManagerMono.StartCoroutine(CustomerCoroutine);
    }

    private void GenerateMeal()
    {
        int Meal_ID = Random.Range(0, 4);
        switch (Meal_ID)
        {
            case 0:
                Meal = Constants.PHO;
                break;

            case 1:
                Meal = Constants.SUSHI;
                break;

            case 2:
                Meal = Constants.BAO_BUNS;
                break;

            case 3:
                Meal = Constants.MANGO_STICKY_RICE;
                break;
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();

        SetAnimationState(CustomerAnimationState);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (Registry.LevelManagerObject != null)
        {
            Registry.LevelManagerObject.CustomersInScene--;
        }

        if (CurrentLocation == Constants.RESTAURANT && Meal != "")
        {
            Registry.MaxScore += 100;
        }
    }
}
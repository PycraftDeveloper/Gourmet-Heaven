using System.Collections;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public CustomerCore _CustomerCore;

    public IEnumerator CustomerCoroutine = null;
    private MonoBehaviour GameManagerMono;

    public string Meal;
    public string CustomerCoroutineDescription = Constants.NO_COROUTINE;

    public Vector2 CurrentVelocity = Vector2.zero;

    private int CustomerAnimationState;

    public bool MealPlaced = false;

    public GameObject[] OrderPopUpMessages = new GameObject[5];
    public GameObject InstantiatedOrderPopUpMessages;

    private void HandleCustomerTouched(Vector2 TouchPosition)
    {
        bool CorrectMealServed = false;
        Vector2 WorldPosition = Camera.main.ScreenToWorldPoint(TouchPosition);
        Collider2D RaycastHit = Physics2D.OverlapPoint(WorldPosition);

        if (RaycastHit != null && RaycastHit.transform == transform)
        {
            if (Registry.PlayerObject.HoldingMeal == Meal)
            {
                CorrectMealServed = true;
                Registry.PlayerScore += 100;
            }

            Registry.PlayerObject.HoldingMeal = Constants.NOT_HOLDING_MEAL;
            int[] CustomerSecondMealRange;
            if (Registry.LevelNumber == Constants.LEVEL_ONE)
            {
                CustomerSecondMealRange = Constants.CUSTOMER_LEVEL_ONE_SECOND_MEAL_RANGE;
            }
            else
            {
                CustomerSecondMealRange = Constants.CUSTOMER_LEVEL_TWO_SECOND_MEAL_RANGE;
            }
            Destroy(InstantiatedOrderPopUpMessages);
            bool DoSecondMeal = Random.Range(CustomerSecondMealRange[0], CustomerSecondMealRange[1]) == 0;

            if (DoSecondMeal && CorrectMealServed)
            {
                GenerateMeal();
                _CustomerCore.Patience = Random.Range(
            Constants.CUSTOMER_MIN_PATIENCE[Registry.LevelNumber],
            Constants.CUSTOMER_MAX_PATIENCE[Registry.LevelNumber]);
            }
            else
            {
                Meal = "";
                _CustomerCore.Patience = 0;
            }
        }
    }

    private void Update()
    {
        if (_CustomerCore.CurrentLocation == Constants.RESTAURANT && InstantiatedOrderPopUpMessages != null && Registry.PlayerObject.HoldingMeal != Constants.NOT_HOLDING_MEAL)
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
    }

    private void Awake()
    {
        _CustomerCore = GetComponent<CustomerCore>();

        GameManagerMono = Registry.GameManagerObject.GetComponent<MonoBehaviour>();

        GenerateMeal();
        SetAnimationState(Constants.CUSTOMER_WALK_SIDE_ANIMATION);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (_CustomerCore.CurrentLocation == Constants.RESTAURANT && InstantiatedOrderPopUpMessages == null)
            {
                Vector2 PopUpPosition = _CustomerCore.CurrentPosition;
                PopUpPosition.y += 0.3f + _CustomerCore._Renderer.bounds.size.y / 2.0f;

                if (Meal == Constants.BAO_BUNS)
                {
                    InstantiatedOrderPopUpMessages = Instantiate(OrderPopUpMessages[0], PopUpPosition, transform.rotation);
                }
                else if (Meal == Constants.MANGO_STICKY_RICE)
                {
                    InstantiatedOrderPopUpMessages = Instantiate(OrderPopUpMessages[1], PopUpPosition, transform.rotation);
                }
                else if (Meal == Constants.PHO)
                {
                    InstantiatedOrderPopUpMessages = Instantiate(OrderPopUpMessages[2], PopUpPosition, transform.rotation);
                }
                else if (Meal == Constants.SUSHI)
                {
                    InstantiatedOrderPopUpMessages = Instantiate(OrderPopUpMessages[3], PopUpPosition, transform.rotation);
                }

                if (_CustomerCore.CustomerTablePosition % 2 == 1 && InstantiatedOrderPopUpMessages != null)
                {
                    Vector2 PopUpScale = InstantiatedOrderPopUpMessages.transform.localScale;
                    PopUpScale.x *= -1;
                    InstantiatedOrderPopUpMessages.transform.localScale = PopUpScale;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_CustomerCore.CurrentLocation == Constants.RESTAURANT && InstantiatedOrderPopUpMessages != null)
        {
            Destroy(InstantiatedOrderPopUpMessages);
        }
    }

    public void SetAnimationState(int StateNumber)
    {
        if (_CustomerCore == null)
        {
            return;
        }
        CustomerAnimationState = StateNumber;
        _CustomerCore._Animator.SetInteger("customerState", CustomerAnimationState);
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
        if (_CustomerCore._Renderer == null)
        {
            _CustomerCore._Renderer = GetComponent<Renderer>();
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

    public void OnEnable()
    {
        SetAnimationState(CustomerAnimationState);
    }

    public void OnDestroy()
    {
        if (Registry.LevelManagerObject != null)
        {
            Registry.LevelManagerObject.CustomersInScene--;
        }
    }
}
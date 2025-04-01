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

    private void Awake()
    {
        _CustomerCore = GetComponent<CustomerCore>();

        GameManagerMono = Registry.GameManagerObject.GetComponent<MonoBehaviour>();

        GenerateMeal();
        SetAnimationState(Constants.CUSTOMER_WALK_SIDE_ANIMATION);
    }

    public void SetAnimationState(int StateNumber)
    {
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
        Registry.LevelManagerObject.CustomersInScene--;
    }
}
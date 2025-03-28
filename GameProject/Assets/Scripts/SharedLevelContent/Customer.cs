using System.Collections;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public Renderer CustomerSprite;
    public Rigidbody2D CustomerRigidBody;
    public IEnumerator CustomerCoroutine = null;
    private MonoBehaviour GameManagerMono;
    public Animator _Animator;

    public string CurrentLocation;
    public string Facing = Constants.FACE_SIDE;
    public string Meal;
    public string CustomerCoroutineDescription = Constants.NO_COROUTINE;

    public Vector2 CurrentPosition = Vector2.zero;
    public Vector2 CurrentVelocity = Vector2.zero;

    private int model_index;
    private int CustomerAnimationState;

    public bool MealPlaced = false;

    public void SetState(int StateNumber)
    {
        CustomerAnimationState = (4 * model_index) + StateNumber;
        Debug.Log(CustomerAnimationState);
        _Animator.SetInteger("customerState", CustomerAnimationState);
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
        if (CustomerSprite == null)
        {
            CustomerSprite = GetComponent<Renderer>();
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

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        CustomerSprite = GetComponent<Renderer>();
        CustomerRigidBody = GetComponent<Rigidbody2D>();
        _Animator = GetComponent<Animator>();

        GameManagerMono = Registry.GameManagerObject.GetComponent<MonoBehaviour>();

        CurrentPosition = transform.position;
        model_index = Random.Range(0, 9);
        CurrentLocation = Constants.KITCHEN;
        GenerateMeal();
        SetState(0);
    }

    public void OnEnable()
    {
        SetState(CustomerAnimationState);
    }

    private void FixedUpdate()
    {
        CustomerRigidBody.position = CurrentPosition;
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Customer : MonoBehaviour
{
    public Renderer CustomerSprite;
    public Rigidbody2D CustomerRigidBody;
    public IEnumerator CustomerCoroutine = null;
    private MonoBehaviour GameManagerMono;
    public Animator _Animator;

    public RuntimeAnimatorController[] AnimationControllers = new RuntimeAnimatorController[8];

    public string CurrentLocation;
    public string Meal;
    public string CustomerCoroutineDescription = Constants.NO_COROUTINE;

    public Vector2 CurrentPosition = Vector2.zero;
    public Vector2 CurrentVelocity = Vector2.zero;

    private int model_index;
    private int CustomerAnimationState;
    private int CustomerAnimationSubCat;
    public int CustomerTablePosition;

    public bool MealPlaced = false;
    public bool DeSpawn = false;

    public float Patience;

    public void SetAnimationState(int StateNumber)
    {
        CustomerAnimationSubCat = StateNumber;
        CustomerAnimationState = StateNumber;
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
        model_index = Random.Range(0, 8);
        _Animator.runtimeAnimatorController = AnimationControllers[model_index];

        //_Animator.keepAnimatorStateOnDisable = true; // should be OK.

        GameManagerMono = Registry.GameManagerObject.GetComponent<MonoBehaviour>();

        CurrentPosition = transform.position;
        CurrentLocation = Constants.KITCHEN;
        GenerateMeal();
        SetAnimationState(Constants.CUSTOMER_WALK_SIDE_ANIMATION);
    }

    public void OnEnable()
    {
        //Debug.Log("====");
        //Debug.Log(model_index);
        //Debug.Log(CustomerAnimationSubCat);
        //SetAnimationState(CustomerAnimationSubCat);

        //_Animator.Rebind();  // Resets the Animator to its default state
        //_Animator.Update(0f); // Forces an immediate frame update
        //_Animator.Play("CustomerAnimatorStartState", -1, 0f); // Restart animation
        //_Animator.enabled = false;
        //_Animator.enabled = true;
        SetAnimationState(CustomerAnimationSubCat); // none of this is the issue
    }

    public void ReAssociateAnimations() // called when 'SetActive(true)'
    {
        //_Animator.enabled = true;
        //_Animator.Rebind();
        //_Animator.Play("CustomerAnimatorStartState", -1, 0f);
        //SetAnimationState(CustomerAnimationSubCat);
        ////_Animator.Update(0f);
        ///
    }

    private void FixedUpdate()
    {
        CustomerRigidBody.position = CurrentPosition;
    }

    private void Update()
    {
        if (Patience != 0)
        {
            Patience -= Time.deltaTime;
            if (Patience <= 0)
            {
                DeSpawn = true;
            }
        }
    }
}
using System.Collections;
using UnityEngine;

public class ForegroundCustomer : CustomerCore // This class extends the customer core behaviour for customers that are interactable by the end user.
{
    public IEnumerator CustomerCoroutine = null; // Store the currently running coroutine for the customer.

    public string Meal; // Store the meal the customer wants.
    public string CustomerCoroutineDescription = Constants.NO_COROUTINE; // Store the description of the currently running coroutine.

    public Vector2 CurrentVelocity = Vector2.zero; // Store the current velocity of the customer, used in moving them in the kitchen queue.

    public bool MealPlaced = false; // Determine if the customer has placed their meal with the player or not.

    public GameObject[] OrderPopUpMessages = new GameObject[4]; // Stores a list of all the different 'meal thought bubbles' prefabs the player can have

    // to indicate which meal the customer needs to be served.
    public GameObject InstantiatedOrderPopUpMessage; // Stores the instance of the OrderPopUpMessages.

    private int MealNumber = 0; // Used to determine how many meals the customer has ordered.

    public Animator PatienceMeterAnimator;

    public IEnumerator MoveInQueue(Vector2 DestinationPosition) // Used to ensure the customers move correctly in the kitchen queue.
    {
        Vector2 TargetPosition = CurrentPosition;

        if (TargetPosition.x != DestinationPosition.x) // If the customer needs to move, then continue - otherwise maintain the current animation state.
        {
            SetAnimationState(Constants.CUSTOMER_WALK_SIDE_ANIMATION); // Start the customer walking sideways.

            while (CurrentPosition.x - (Constants.CUSTOMER_MOVEMENT_SPEED * Time.deltaTime) > DestinationPosition.x && gameObject != null) // Move the customer in the x axis until their
                                                                                                                                           // next update puts them too far, and also stop updating if the context changes.
            {
                CurrentPosition.x -= Constants.CUSTOMER_MOVEMENT_SPEED * Time.deltaTime; // Move the player linearly using a constant movement speed.
                yield return null;
            }

            if (CurrentPosition.x - (Constants.CUSTOMER_MOVEMENT_SPEED * Time.deltaTime) < DestinationPosition.x) // If the customer hasn't moved into the exact location, force them into position here.
            {
                CurrentPosition = DestinationPosition;
            }

            SetAnimationState(Constants.CUSTOMER_IDLE_SIDE_ANIMATION); // Then reset the animation state to idle sideways.
        }
    }

    public IEnumerator MoveIntoRestaurant() // Used to give the illusion of customers walking out the bottom of the Kitchen, into the restaurant.
    {
        SetAnimationState(Constants.CUSTOMER_WALK_DOWN_ANIMATION); // Set the animation to walking.
        while (gameObject != null && CurrentPosition.y > -6.31) // Whilst the player is in the same scene, and the customer hasn't been deleted
                                                                // which is mainly used for stopping execution through the editor and handling errors there.
        {
            CurrentPosition = new Vector2(
                CurrentPosition.x,
                CurrentPosition.y - Constants.CUSTOMER_MOVEMENT_SPEED * Time.deltaTime); // move the customer down the scene.
            yield return null;
        }
        CurrentPosition.y = -6.31f; // Set the customer to be at the bottom of the kitchen scene.
        PlaceIntoRestaurant(); // Set the customer up in the restaurant.
    }

    private void PlaceIntoRestaurant() // Similar to before, this is used to place customers in the restaurant scene.
                                       // HOWEVER, this is for customers the player can interact with, NOT for background customers.
    {
        bool Seated = false;
        while (!Seated) // Position the customer at a random empty seat.
        {
            int PositionIndex = Random.Range(0, 8); // Get the index for a random table.
            GameObject[] CustomerTableArrangement = Registry.LevelManagerObject.CustomerTableArrangement;
            if (CustomerTableArrangement[PositionIndex] == null || CustomerTableArrangement[PositionIndex].GetComponent<BackgroundCustomer>()) // Ensure the table is empty - or a background customer is sat there
                                                                                                                                               // They can be overridden by more important customers.
            {
                if (CustomerTableArrangement[PositionIndex] != null) // If the seat is occupied by a background customer, remove them.
                {
                    BackgroundCustomer BackgroundCustomer = CustomerTableArrangement[PositionIndex].GetComponent<BackgroundCustomer>();
                    if (BackgroundCustomer)
                    {
                        Registry.BackgroundCustomers.Remove(BackgroundCustomer);
                        Destroy(CustomerTableArrangement[PositionIndex]);
                        CustomerTableArrangement[PositionIndex] = null;
                    }
                }
                if (PositionIndex % 2 == 0) // If the customer is sat on left side of the table, flip their scale to ensure they face towards the table.
                {
                    Vector3 CustomerScale = transform.localScale;
                    CustomerScale.x *= -1;
                    transform.localScale = CustomerScale;
                }
                SetAnimationState(Constants.CUSTOMER_IDLE_SIT_ANIMATION); // Ensure that when placed into the restaurant, the customer is in the sitting animation
                SetupCustomerCoreForRestaurant(PositionIndex); // Set up the customer core for the restaurant.
                Seated = true; // Stop the while loop now a seat has been found.
                CustomerSeated = true; // Set the customer to be seated in the restaurant.
            }
        }
    }

    private void HandleCustomerTouched(Vector2 TouchPosition) // Determine if the customer has been interacted with by the end-user. This behaviour seems a bit flaky,
                                                              // but don't have the time to investigate further as yet.
    {
        bool CorrectMealServed = false; // Determine if the customer has been given the correct meal.
        Vector2 WorldPosition = Camera.main.ScreenToWorldPoint(TouchPosition);
        Collider2D RaycastHit = Physics2D.OverlapPoint(WorldPosition);

        if (RaycastHit != null && RaycastHit.transform == transform) // Use a ray-cast to check if the end-user tapped a customer.
        {
            if (Registry.PlayerObject.HoldingMeal == Meal) // If the player is holding the correct meal
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
                Registry.PlayerScore += 100; // Award the player with points for serving the customer.
            }

            // Regardless of if the customer was given the correct meal or not...

            Registry.PlayerObject.HoldingMeal = Constants.NOT_HOLDING_MEAL; // Set the player to not holding a meal.

            Destroy(InstantiatedOrderPopUpMessage); // Destroy the order pop-up message, as the customer's order is about to change.

            int[] CustomerSecondMealRange = Constants.CUSTOMER_LEVEL_TWO_SECOND_MEAL_RANGE; // Determine the probability of the customer ordering a second meal.
            bool DoSecondMeal = Random.Range(CustomerSecondMealRange[0], CustomerSecondMealRange[1]) == 0; // Determine if the customer wants a second meal.

            MealNumber++; // Increase the number of meals by one.

            if (Registry.LevelNumber == Constants.LEVEL_ONE)
            {
                DoSecondMeal = false; // Only one meal on level one
            }
            else
            {
                DoSecondMeal = DoSecondMeal && MealNumber < Constants.LEVEL_TWO_CUSTOMER_MAX_ORDERS; // Can order more than one meal in level two. (to a max of two, min still is one).
            }

            if (DoSecondMeal && CorrectMealServed) // If the customer wants a second meal and can have it, then generate a new meal and reset the patience.
            {
                GenerateMeal();
                Patience = Random.Range(
            Constants.CUSTOMER_MIN_PATIENCE[Registry.LevelNumber],
            Constants.CUSTOMER_MAX_PATIENCE[Registry.LevelNumber]);
                InitialPatience = Patience;
                MealNumber++;

                if (PatienceMeterAnimator != null) // Also reset the patience meter.
                {
                    PatienceMeterAnimator.StopPlayback();
                    PatienceMeterAnimator.Play("PatienceStart", 0, 1.0f - (Patience / InitialPatience));
                }
            }
            else // Mark the customer for garbage collection when player leaves restaurant.
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
        ManagePatience(); // Call the base class function to manage the customer's patience.
        if (InstantiatedOrderPopUpMessage != null && Registry.PlayerObject.HoldingMeal != Constants.NOT_HOLDING_MEAL)
        {
            // If the player is trying to serve customers in the restaurant, and is within the customer's hit box (see below).
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

        GenerateMeal();
        SetAnimationState(Constants.CUSTOMER_WALK_SIDE_ANIMATION); // Customers spawn into the kitchen and immediately need to walk in the queue.
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (InstantiatedOrderPopUpMessage == null && DeSpawn == false) // If the customer is not set to de-spawn, and is in the restaurant, and not yet been inetracted with
            {
                //Vector2 PopUpPosition = CurrentPosition;
                //PopUpPosition.y += 0.3f + _Renderer.bounds.size.y / 2.0f;

                // Instantiate the appropriate pop-up according to what the customer wants.
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

                // If on the right, flip the pop-up so it is positioned correctly on-screen.
                if (CustomerTablePosition % 2 == 1 && InstantiatedOrderPopUpMessage != null)
                {
                    Vector2 PopUpScale = InstantiatedOrderPopUpMessage.transform.localScale;
                    PopUpScale.x *= -1;
                    InstantiatedOrderPopUpMessage.transform.localScale = PopUpScale;
                }

                // Set the pop-up message to be a child of the customer object, so it is automatically managed when the scene changes.
                InstantiatedOrderPopUpMessage.transform.SetParent(transform, true);

                Vector2 Position = new Vector2(-0.28f, 1.24f); // Position the pop-up message above the customer.
                InstantiatedOrderPopUpMessage.transform.localPosition = Position;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (InstantiatedOrderPopUpMessage != null)
        {
            Destroy(InstantiatedOrderPopUpMessage); // When the player leaves the customer's hit box, hide the pop-up message.
        }
    }

    public void SetCoroutine(string description) // Part of an overloaded set of functions designed to customise coroutine behaviour.
    {
        if (description == Constants.NO_COROUTINE)
        {
            Registry.GameManagerObject.StopCoroutine(CustomerCoroutine);
            CustomerCoroutine = null;
            CustomerCoroutineDescription = Constants.NO_COROUTINE;
        }
    }

    public void SetCoroutine(IEnumerator coroutine, string description) // Used to determine which co-routine is allowed to run if there are conflicting ones.
    {
        if (CustomerCoroutine == null)
        {
            CustomerCoroutine = coroutine;
            CustomerCoroutineDescription = description;
        }
        else if (description == Constants.MOVE_INTO_RESTAURANT && CustomerCoroutineDescription == Constants.MOVE_IN_QUEUE)
        {
            Registry.GameManagerObject.StopCoroutine(CustomerCoroutine);
            CustomerCoroutine = coroutine;
            CustomerCoroutineDescription = description;
        }
        else if (description == Constants.MOVE_IN_QUEUE && CustomerCoroutineDescription == Constants.MOVE_IN_QUEUE)
        {
            Registry.GameManagerObject.StopCoroutine(CustomerCoroutine);
            CustomerCoroutine = coroutine;
        }
        if (_Renderer == null)
        {
            _Renderer = GetComponent<Renderer>();
        }
        Registry.GameManagerObject.StartCoroutine(CustomerCoroutine);
    }

    private void GenerateMeal() // Generate a new random meal for the customer.
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

    protected void OnDestroy()
    {
        if (Registry.LevelManagerObject != null)
        {
            Registry.LevelManagerObject.CustomersInScene--; // Ensure that when the customer is destroyed, the number of customer's in the game scenes is reduced.
        }
    }
}
public class BackgroundCustomer : CustomerCore // This class extends the core customer behaviour for customer objects that are not interactive
                                               // in the restaurant scene. This is much simpler than the normal customer class.
{
    protected override void Awake()
    {
        base.Awake();

        _Animator.SetInteger("customerState", Constants.CUSTOMER_IDLE_SIT_ANIMATION); // Set the animation to idle sit.

        CurrentLocation = Constants.RESTAURANT; // Set the location to restaurant.
    }

    private void Update()
    {
        _Animator.SetInteger("customerState", Constants.CUSTOMER_IDLE_SIT_ANIMATION); // Keep the customer in the idle sitting animation until they de-spawn.
    }
}
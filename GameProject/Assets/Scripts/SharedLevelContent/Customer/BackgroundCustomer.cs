public class BackgroundCustomer : CustomerCore
{
    protected override void Awake()
    {
        base.Awake();

        _Animator.SetInteger("customerState", Constants.CUSTOMER_IDLE_SIT_ANIMATION);

        CurrentLocation = Constants.RESTAURANT;
    }

    private void Update()
    {
        _Animator.SetInteger("customerState", Constants.CUSTOMER_IDLE_SIT_ANIMATION);
    }
}
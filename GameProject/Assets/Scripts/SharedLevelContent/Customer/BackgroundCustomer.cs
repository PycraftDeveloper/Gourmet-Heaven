using UnityEngine;

public class BackgroundCustomer : MonoBehaviour
{
    public CustomerCore _CustomerCore;

    private void Awake()
    {
        _CustomerCore = GetComponent<CustomerCore>();

        _CustomerCore._Animator.SetInteger("customerState", Constants.CUSTOMER_IDLE_SIT_ANIMATION);

        _CustomerCore.CurrentLocation = Constants.RESTAURANT;
    }

    private void Update()
    {
        _CustomerCore._Animator.SetInteger("customerState", Constants.CUSTOMER_IDLE_SIT_ANIMATION);
    }
}
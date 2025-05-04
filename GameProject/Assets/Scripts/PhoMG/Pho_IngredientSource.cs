using UnityEngine;

public class Pho_IngredientSource : MonoBehaviour
{
    private Rigidbody2D IngredientRigidBody;

    private float X_Velocity;

    private void Start()
    {
        IngredientRigidBody = GetComponent<Rigidbody2D>();

        X_Velocity = Random.Range(Constants.PHO_FOOD_ITEMS_SPEED[0], Constants.PHO_FOOD_ITEMS_SPEED[1]);
        if (Random.Range(0, 2) == 0)
        {
            X_Velocity *= -1;
        }
        IngredientRigidBody.linearVelocityX = X_Velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (X_Velocity < 0)
        {
            X_Velocity = Random.Range(Constants.PHO_FOOD_ITEMS_SPEED[0], Constants.PHO_FOOD_ITEMS_SPEED[1]);
        }
        else
        {
            X_Velocity = -Random.Range(Constants.PHO_FOOD_ITEMS_SPEED[0], Constants.PHO_FOOD_ITEMS_SPEED[1]);
        }

        IngredientRigidBody.linearVelocityX = X_Velocity;
    }
}
using UnityEngine;

public class Pho_IngredientSource : MonoBehaviour
// This class is assigned to the ingredient bowl and moves horizontally back and forth across the screen.
{
    private Rigidbody2D IngredientRigidBody;

    private float X_Velocity;

    private void Start()
    {
        IngredientRigidBody = GetComponent<Rigidbody2D>();

        X_Velocity = Random.Range(Constants.PHO_FOOD_ITEMS_SPEED[0], Constants.PHO_FOOD_ITEMS_SPEED[1]); // Generate a random velocity to make mini-game harder
        if (Random.Range(0, 2) == 0)
        {
            X_Velocity *= -1; // Randomise initial direction
        }
        IngredientRigidBody.linearVelocityX = X_Velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PhoBorderLeft"))
        {
            X_Velocity = Random.Range(Constants.PHO_FOOD_ITEMS_SPEED[0], Constants.PHO_FOOD_ITEMS_SPEED[1]);
            IngredientRigidBody.MovePosition(IngredientRigidBody.position + new Vector2(0.1f, 0f));
            IngredientRigidBody.linearVelocityX = X_Velocity;
        }
        else if (collision.gameObject.CompareTag("PhoBorderRight"))
        {
            X_Velocity = -Random.Range(Constants.PHO_FOOD_ITEMS_SPEED[0], Constants.PHO_FOOD_ITEMS_SPEED[1]);
            IngredientRigidBody.MovePosition(IngredientRigidBody.position + new Vector2(-0.1f, 0f));
            IngredientRigidBody.linearVelocityX = X_Velocity;
        }
    }
}
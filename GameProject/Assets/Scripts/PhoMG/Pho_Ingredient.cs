using UnityEngine;

public class Pho_Ingredient : MonoBehaviour
{
    private Rigidbody2D IngredientRigidBody;

    public bool DropIngredient;
    public bool MissedTarget = false;

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
        IngredientRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        IngredientRigidBody.constraints = RigidbodyConstraints2D.None;
    }

    private void FixedUpdate()
    {
        Vector2 IngredientPosition = IngredientRigidBody.position;
        if (IngredientPosition.y < -6)
        {
            MissedTarget = true;
        }
    }

    private void Update()
    {
        if (DropIngredient)
        {
            IngredientRigidBody.gravityScale = 1;
            DropIngredient = false;
        }
    }
}
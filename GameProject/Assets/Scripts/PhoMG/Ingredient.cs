using UnityEngine;

public class PhoIngredient : MonoBehaviour
{
    private Rigidbody2D IngredientRigidBody;
    private bool Falling = false;
    public int IngredientID;

    private void Start()
    {
        IngredientRigidBody = GetComponent<Rigidbody2D>();

        Vector2 Velocity = Vector2.zero;
        Velocity.x = Random.Range(Constants.PHO_FOOD_ITEMS_SPEED[0], Constants.PHO_FOOD_ITEMS_SPEED[1]);
        IngredientRigidBody.linearVelocity = Velocity;

        IngredientID = Random.Range(0, 3);
    }

    private void FixedUpdate()
    {
        Vector2 Position = IngredientRigidBody.position;

        if (!Falling)
        {
            if (Position.x > 8.38f) // On contact with the left side of the screen.
            {
                Vector2 Velocity = IngredientRigidBody.linearVelocity;
                Velocity.x = -Random.Range(Constants.PHO_FOOD_ITEMS_SPEED[0], Constants.PHO_FOOD_ITEMS_SPEED[1]);
                IngredientRigidBody.linearVelocity = Velocity;

                Position.x = 8.38f;
                IngredientRigidBody.position = Position;
            }
            else if (Position.x < -8.38f) // On contact with the right side of the screen.
            {
                Vector2 Velocity = IngredientRigidBody.linearVelocity;
                Velocity.x = Random.Range(Constants.PHO_FOOD_ITEMS_SPEED[0], Constants.PHO_FOOD_ITEMS_SPEED[1]);
                IngredientRigidBody.linearVelocity = Velocity;

                Position.x = -8.38f;
                IngredientRigidBody.position = Position;
            }
        }
        else
        {
            if (IngredientRigidBody.gravityScale != 1.0f)
            {
                IngredientRigidBody.linearVelocity = Vector2.zero;
                IngredientRigidBody.gravityScale = 1.0f;
            }

            if (Position.y < -5.5f)
            {
                Destroy(gameObject);
                // Spawn new ingredient here too
            }
        }
    }

    private void Update()
    {
        if (Input.touchCount > 0) // if touch input is used
        {
            Falling = true;
        }
        else if (Input.GetMouseButton(0)) // if mouse click is used
        {
            Falling = true;
        }
    }
}
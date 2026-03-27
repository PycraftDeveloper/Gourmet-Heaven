using UnityEngine;

public class Pho_Ingredient : MonoBehaviour
// This class is assigned to the ingredient that falls out of the ingredient bowl, and is used to determine if the food made it into the bowl or not.
{
    private Rigidbody2D IngredientRigidBody;

    public bool MissedTarget = false; // Used externally to trigger the mini-game to fail when target missed.

    private void Start()
    {
        IngredientRigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector2 IngredientPosition = IngredientRigidBody.position;
        if (IngredientPosition.y < -6) // Determine if the ingredient has fallen off the bottom of the screen (and therefore missed the bowl).
        {
            MissedTarget = true;
        }
    }
}
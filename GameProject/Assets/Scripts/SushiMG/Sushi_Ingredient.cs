using UnityEngine;

public class Sushi_Ingredient : MonoBehaviour // This script runs on the ingredient that spawns and that you drag into the sushi mat.
{
    public bool InTarget = false; // Used to determine if the ingredient is in the target area.
    private Rigidbody2D IngredientRigidbody; // Get the ingredient rigid body to move it around the screen and detect collisions.
    private Vector2 IngredientPosition; // Used to store where the ingredient is based on the input position on-screen.

    private void Start()
    {
        IngredientRigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetPosition(Vector2 position) // Set position based on input position
    {
        IngredientPosition = Camera.main.ScreenToWorldPoint(position);
    }

    private void FixedUpdate() // Ensure the position of the object reflects the input position.
    {
        IngredientRigidbody.MovePosition(IngredientPosition);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "TargetHitbox") // If the ingredient has been dragged into the target
        {
            InTarget = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "TargetHitbox") // If the ingredient has been dragged out of the target
        {
            InTarget = false;
        }
    }
}
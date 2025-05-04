using UnityEngine;

public class Sushi_Ingredient : MonoBehaviour
{
    public bool InTarget = false;
    private Rigidbody2D IngredientRigidbody;
    private Vector2 IngredientPosition;

    private void Start()
    {
        IngredientRigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetPosition(Vector2 position)
    {
        IngredientPosition = Camera.main.ScreenToWorldPoint(position);
    }

    private void FixedUpdate()
    {
        IngredientRigidbody.MovePosition(IngredientPosition);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "TargetHitbox")
        {
            InTarget = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "TargetHitbox")
        {
            InTarget = false;
        }
    }
}
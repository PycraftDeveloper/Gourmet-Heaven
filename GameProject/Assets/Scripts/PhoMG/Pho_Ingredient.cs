using UnityEngine;

public class Pho_Ingredient : MonoBehaviour
{
    private Rigidbody2D IngredientRigidBody;

    public bool MissedTarget = false;

    private void Start()
    {
        IngredientRigidBody = GetComponent<Rigidbody2D>();
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
    }
}
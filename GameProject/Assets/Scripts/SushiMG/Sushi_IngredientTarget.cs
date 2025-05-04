using UnityEngine;

public class Sushi_IngredientTarget : MonoBehaviour
{
    public string IngredientName;
    public SpriteRenderer IngredientRenderer;

    private void OnTriggerStay2D(Collider2D Collision)
    {
        if (Collision.name == IngredientName)
        {
            IngredientRenderer.enabled = true;
        }
    }

    private void Awake()
    {
        IngredientRenderer = GetComponent<SpriteRenderer>();
    }
}
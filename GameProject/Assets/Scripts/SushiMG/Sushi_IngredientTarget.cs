using UnityEngine;

public class Sushi_IngredientTarget : MonoBehaviour // This is attached to the sushi rolling mat's updated states, the target for which a specific ingredient should be added.
{
    public string IngredientName; // The name of the ingredient that needs to be dragged into the target area (used to determine if the correct ingredient is dragged in).
    public SpriteRenderer IngredientRenderer; // Get the sprite renderer to show the ingredient when it is dragged into the target area.

    private void OnTriggerStay2D(Collider2D Collision)
    {
        if (Collision.name == IngredientName)
        {
            IngredientRenderer.enabled = true; // If the correct ingredient is dragged into the target area, show the updated sushi-rolling mat with the new assets.
        }
    }

    private void Awake()
    {
        IngredientRenderer = GetComponent<SpriteRenderer>();
    }
}
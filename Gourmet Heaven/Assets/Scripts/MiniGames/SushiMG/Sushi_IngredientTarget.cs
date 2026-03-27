using UnityEngine;

public class Sushi_IngredientTarget : MonoBehaviour // This is attached to the sushi rolling mat's updated states, the target for which a specific ingredient should be added.
{
    public string IngredientName; // The name of the ingredient that needs to be dragged into the target area (used to determine if the correct ingredient is dragged in).

    private void OnTriggerStay2D(Collider2D Collision)
    {
        if (Collision.name == IngredientName)
        {
            gameObject.SetActive(true); // If the correct ingredient is dragged into the target area, show the updated sushi-rolling mat with the new assets.
        }
    }
}
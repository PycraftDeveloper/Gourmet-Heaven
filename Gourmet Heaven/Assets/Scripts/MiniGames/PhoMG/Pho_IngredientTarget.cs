using UnityEngine;

public class Pho_IngredientTarget : MonoBehaviour
// This is attached to the Pho bowl, otherwise considered as the target area that the ingredient needs to collide with for the mini-game to be successful.
{
    public bool IngredientInTargetToggle = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PhoFallingIngredient"))
        {
            Debug.Log("Ingredient entered target area.");
            IngredientInTargetToggle = true; // Determine if anything (in this case the only candidate is a falling ingredient) has entered the target area.
        }
        else
        {
            Debug.Log($"Non-ingredient object entered target area. '{collision.gameObject.tag}'");
        }
    }
}
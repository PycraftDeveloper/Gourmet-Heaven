using UnityEngine;

public class Pho_IngredientTarget : MonoBehaviour
{
    public bool IngredientInTargetToggle = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IngredientInTargetToggle = true;
    }
}
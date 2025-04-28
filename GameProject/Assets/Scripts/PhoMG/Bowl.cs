using UnityEngine;

public class PhoBowl : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        PhoIngredient EnteredIngredient = collision.GetComponent<PhoIngredient>();
        Debug.Log(EnteredIngredient.IngredientID); // From here, determine if should be unique set of ingredients, spawn new ingredients using prefabs
    }
}
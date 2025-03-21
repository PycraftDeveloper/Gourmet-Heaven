using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopUpVisibility : MonoBehaviour
{
    [System.Serializable]
    public class TriggerObjectPair
    {
        public string triggerTag; // Unique tag for each trigger zone
        public GameObject targetObject; // The object we are changing the visibility for
    }

    public TriggerObjectPair[] triggerPairs; // Making the array to hold each pair

    private void OnTriggerEnter2D(Collider2D other) // Detects when player enters trigger
    {
        foreach (TriggerObjectPair pair in triggerPairs) // The code loops through the array and checks if the trigger zone tag matches with one of the ones in the array
        {
            if (other.CompareTag(pair.triggerTag))
            {
                pair.targetObject.SetActive(true); // sets the visibility of the GameObject to on
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) // Detects when the player leaves trigger
    {
        foreach (TriggerObjectPair pair in triggerPairs) // The code loops through the array and checks if the trigger zone tag matches with one of the ones in the array
        {
            if (other.CompareTag(pair.triggerTag))
            {
                pair.targetObject.SetActive(false); // sets the visibility of the GameObject to off
            }
        }
    }
}
// The following program was written by Emmie Heane.

using Unity.VisualScripting;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    private Vector2 swipeStart; // when the slice attempt starts
    private Vector2 swipeEnd; // when the slice attempt ends
    public float minSwipeDistance = 50f; // the minium distance for a slice attempt
    public SlicedObject SlicedObject; // reference to the slicing object script

    // detects when the slicing starts when the mouse click is down or if the players finger is touching the slice points and if the distance of the swipe is long enough
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            swipeStart = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            swipeEnd = Input.mousePosition;
            float swipeDistance = Vector2.Distance(swipeStart, swipeEnd);

            if (swipeDistance >= minSwipeDistance)
            {
                SlicedObject.TrySlice(swipeStart, swipeEnd);
                Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.audioClip2);
            }
            else
            {
                Debug.Log("Swipe too short!");
            }
        }
    }
}
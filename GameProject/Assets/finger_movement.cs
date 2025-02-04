// This script uses Vector2, and considers the depth of the objects separately as this is NOT a 3D object.

using Unity.VisualScripting;
using UnityEngine;

public class PlayerInputCircle : MonoBehaviour // This class defines behaviour for mouse/touch interaction
{
    public GameObject parent; // The game object this belongs to: 'Joystick'
    public GameObject background; // The background for the joystick.
    public Camera mainCamera; // The camera used for the scene
    private bool interacted = false; // used to determine if the user has interacted with the widget.

    private float Pythagoreas_Theorum(Vector2 position_one, Vector2 position_two) // Finds the magnitude of the difference between two vectors.
    {
        Vector2 magnitude_vector = new Vector2(Mathf.Abs(position_two.x - position_one.x), Mathf.Abs(position_two.y - position_one.y));
        return magnitude_vector.magnitude;
    }

    private void Set_Sprite_Position_From_Input_Position(Vector2 input_position) // Constrains the user input to the background area. Also handles dragging outside background area.
    {
        Vector2 touch_position = mainCamera.ScreenToWorldPoint(new Vector2(input_position.x, input_position.y)); // converts the screen position to world positions.
        Vector2 parent_position = new Vector2(parent.transform.position.x, parent.transform.position.y); // gets centre of joystick object.
        float radius = background.transform.localScale.x / 2; // gets the radius of the background.
        float distance_from_centre = Pythagoreas_Theorum(touch_position, parent_position); // gets current distance from centre position.

        if (distance_from_centre < radius)
        {
            interacted = true; // if input is inside joystick area, allow the user to use joystick.
        }

        if (interacted) // if player is using joystick
        {
            if (distance_from_centre > radius) // if the position is outside the background, fix the centre to the radius of the background.
            {
                touch_position = parent_position + (touch_position - parent_position).normalized * radius;
            }
        }
        else
        {
            touch_position = parent_position; // reset position
        }

        transform.position = new Vector3(touch_position.x, touch_position.y, transform.position.z); // set the position of the object, using sprites original depth.
    }

    private void Start()
    {
        transform.position = parent.transform.position; // set the position to the centre of the background.
    }

    private void Update()
    {
        if (Input.touchCount > 0) // if touch input is used
        {
            UnityEngine.Touch touch = Input.GetTouch(0); // get the first finger (only guaranteed input)

            Set_Sprite_Position_From_Input_Position(touch.position); // handle position adjustments, using finger position on-screen.
        }
        else if (Input.GetMouseButton(0)) // if mouse click is used
        {
            Set_Sprite_Position_From_Input_Position(Input.mousePosition); // handle position adjustments, using mouse position.
        }
        else
        {
            transform.position = parent.transform.position; // reset position to centre.
            interacted = false; // no longer interacted.
        }
    }
}
using UnityEngine;

public class Joystick : MonoBehaviour
{
    public PlayerInputCircle JoystickInput;

    // Store the left and right positions the joystick can be in on-screen. Can be changed in the options menu.
    public Vector2 LeftPosition;

    public Vector2 RightPosition;

    private void Awake() // Ensure the joystick persists across scene changes.
    {
        if (Registry.JoystickObject == null)
        {
            DontDestroyOnLoad(gameObject);
            Registry.JoystickObject = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Registry.JoystickScreenPosition == Constants.LEFT)
        {
            transform.localPosition = LeftPosition;
        }
        else
        {
            transform.localPosition = RightPosition;
        }
    }
}
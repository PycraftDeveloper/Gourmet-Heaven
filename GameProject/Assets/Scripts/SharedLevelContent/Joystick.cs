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

    private void Start()
    {
        OnSceneChanged(); // Ensure the joystick is correctly positioned when the scene starts.
    }

    public void OnSceneChanged() // Called by the game-manager to ensure the joystick is correctly positioned after it has been re-created. (for example if there
                                 // have been changes to the options menu.
    {
        if (Registry.JoystickScreenPosition == Constants.LEFT)
        {
            transform.position = LeftPosition;
        }
        else
        {
            transform.position = RightPosition;
        }
    }
}
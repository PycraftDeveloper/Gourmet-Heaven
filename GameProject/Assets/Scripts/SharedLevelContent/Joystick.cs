using UnityEngine;

public class Joystick : MonoBehaviour
{
    public PlayerInputCircle JoystickInput;

    public Vector2 LeftPosition;
    public Vector2 RightPosition;

    private void Awake()
    {
        if (Registry.JoystickExists == false)
        {
            DontDestroyOnLoad(gameObject);
            Registry.JoystickExists = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
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
using UnityEngine;

public class Joystick : MonoBehaviour
{
    public PlayerInputCircle JoystickInput;

    public Vector2 LeftPosition;
    public Vector2 RightPosition;

    private void Awake()
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
        OnSceneChanged();
    }

    public void OnSceneChanged()
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
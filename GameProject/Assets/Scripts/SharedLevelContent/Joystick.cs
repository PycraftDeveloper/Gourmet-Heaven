using UnityEngine;

public class Joystick : MonoBehaviour
{
    public PlayerInputCircle JoystickInput;

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
}
using UnityEngine;
using UnityEngine.Rendering;

public class texture_exchanger : MonoBehaviour
{
    public Sprite down_texture;
    public Sprite up_texture;
    public Sprite left_texture;
    public Sprite right_texture;

    private SpriteRenderer MySprite;

    public GameObject JoystickInputObject;
    private PlayerInputCircle JoystickInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        MySprite = GetComponent<SpriteRenderer>();
        JoystickInput = JoystickInputObject.GetComponent<PlayerInputCircle>();
    }

    // Update is called once per frame
    private void Update()
    {
        Vector2 JoystickInputMagnitude = JoystickInput.JoystickOffsetMagnitude;

        if (Mathf.Abs(JoystickInputMagnitude.x) > Mathf.Abs(JoystickInputMagnitude.y))
        {
            if (JoystickInputMagnitude.x > 0)
            {
                MySprite.sprite = right_texture;
            }
            else if (JoystickInputMagnitude.x < 0)
            {
                MySprite.sprite = left_texture;
            }
        }
        else
        {
            if (JoystickInputMagnitude.y > 0)
            {
                MySprite.sprite = up_texture;
            }
            else if (JoystickInputMagnitude.y < 0)
            {
                MySprite.sprite = down_texture;
            }
        }
    }
}
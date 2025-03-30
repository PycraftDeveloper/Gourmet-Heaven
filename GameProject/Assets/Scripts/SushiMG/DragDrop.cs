using UnityEngine;

public class DragDrop : MonoBehaviour
{
    private Vector3 OriginalPosition;

    private Camera mainCamera;
    private Collider2D Hitbox;

    public bool IsMovable = false;

    private void Start()
    {
        mainCamera = Camera.main;
        Hitbox = GetComponent<Collider2D>();

        OriginalPosition = transform.position;
    }

    private void Set_Sprite_Position_From_Input_Position(Vector2 InputPosition)
    {
        Vector3 Position = transform.position;
        Position.x = InputPosition.x;
        Position.y = InputPosition.y;
        transform.position = Position;
    }

    public void Reuse()
    {
        transform.position = OriginalPosition;
        IsMovable = false;
    }

    private void Update()
    {
        Vector2 InputPosition = Vector2.zero;
        bool UserInput = false;

        if (Input.touchCount > 0)
        {
            UserInput = true;

            UnityEngine.Touch touch = Input.GetTouch(0);

            InputPosition = mainCamera.ScreenToWorldPoint(new Vector2(touch.position.x, touch.position.y));
        }
        else if (Input.GetMouseButton(0))
        {
            UserInput = true;

            InputPosition = mainCamera.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        }

        if (UserInput)
        {
            if (IsMovable == false)
            {
                if (Hitbox.OverlapPoint(InputPosition))
                {
                    IsMovable = true;
                }
            }
            else
            {
                Set_Sprite_Position_From_Input_Position(InputPosition);
            }
        }
        else
        {
            IsMovable = false;
            transform.position = OriginalPosition;
        }
    }
}
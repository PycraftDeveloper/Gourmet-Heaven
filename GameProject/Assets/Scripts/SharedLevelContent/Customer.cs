using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Customer : MonoBehaviour
{
    public Sprite down_texture;
    public Sprite up_texture;
    public Sprite side_texture;

    public SpriteRenderer CustomerSprite;

    public int RenderPriority = 1;

    public Rigidbody2D CustomerRigidBody;

    public string CurrentLocation = Constants.KITCHEN;

    public string Facing = Constants.FACE_SIDE;

    public Vector2 CurrentPosition = Vector2.zero;
    public Vector2 CurrentVelocity = Vector2.zero;

    public bool WaitingToBeServed = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        CustomerSprite = GetComponent<SpriteRenderer>();
        CustomerRigidBody = GetComponent<Rigidbody2D>();

        CurrentPosition = transform.position;
    }

    private void FixedUpdate()
    {
        CustomerSprite.sortingOrder = RenderPriority;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Facing == Constants.FACE_SIDE)
        {
            CustomerSprite.sprite = side_texture;
        }
        else if (Facing == Constants.FACE_UP)
        {
            CustomerSprite.sprite = up_texture;
        }
        else if (Facing == Constants.FACE_DOWN)
        {
            CustomerSprite.sprite = down_texture;
        }
    }
}
using UnityEngine;

public class Customer : MonoBehaviour
{
    public Sprite[] down_texture = new Sprite[6];
    public Sprite[] up_texture = new Sprite[6];
    public Sprite[] side_texture = new Sprite[6];

    public SpriteRenderer CustomerSprite;

    public int RenderPriority = 1;

    public Rigidbody2D CustomerRigidBody;

    public string CurrentLocation;

    public string Facing = Constants.FACE_SIDE;

    public Vector2 CurrentPosition = Vector2.zero;
    public Vector2 CurrentVelocity = Vector2.zero;

    public bool WaitingToBeServed = false;

    private int model_index;

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
        model_index = Random.Range(0, 6);
        CurrentLocation = Constants.KITCHEN;
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
            CustomerSprite.sprite = side_texture[model_index];
        }
        else if (Facing == Constants.FACE_UP)
        {
            CustomerSprite.sprite = up_texture[model_index];
        }
        else if (Facing == Constants.FACE_DOWN)
        {
            CustomerSprite.sprite = down_texture[model_index];
        }
    }
}
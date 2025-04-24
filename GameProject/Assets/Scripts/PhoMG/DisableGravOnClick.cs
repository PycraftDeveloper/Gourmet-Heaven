using UnityEngine;

public class DisableGravityOnClick2D : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            if (rb != null)
            {
                rb.gravityScale = 0f; // Disable gravity
            }
        }
    }
}

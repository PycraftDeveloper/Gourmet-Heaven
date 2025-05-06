using UnityEngine;

public class BasicRenderPriority : MonoBehaviour
{
    public int RenderPriority;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = RenderPriority;
    }
}
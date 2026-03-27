using UnityEngine;

public class CameraAdjust : MonoBehaviour
{
    private void Start()
    {
        Adjust();
    }

    public void Adjust()
    {
        float targetAspect = 16f / 9f;

        Rect safe = Screen.safeArea;

        float windowAspect = safe.width / safe.height;
        float scaleHeight = windowAspect / targetAspect;

        Camera cam = GetComponent<Camera>();

        if (scaleHeight < 1.0f)
        {
            float height = scaleHeight;
            float y = (1.0f - height) / 2.0f;

            cam.rect = new Rect(0, y, 1, height);
        }
        else
        {
            float width = 1.0f / scaleHeight;
            float x = (1.0f - width) / 2.0f;

            cam.rect = new Rect(x, 0, width, 1);
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public class BackgroundApplier : MonoBehaviour
{
    public RawImage BackgroundImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // use "Registry.GameManagerObject.FrameTexture;" for no blur
        // use "Registry.GameManagerObject.FrameTexture;" for blur
        // use transparency if you want the blur to appear to 'fade in' :D
        BackgroundImage.texture = Registry.GameManagerObject.BlurredFrameTexture;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}
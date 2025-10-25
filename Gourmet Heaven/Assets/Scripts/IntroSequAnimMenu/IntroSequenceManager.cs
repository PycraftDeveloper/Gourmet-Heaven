using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroSequenceManager : MonoBehaviour
{
    public GameObject[] IntroSequenceItems = new GameObject[8]; // The 8 different animations for the comic section.

    public GameObject IntroZoomObject;
    private Image IntroZoomImage;
    public Image AnimationBackground;
    public Camera BackgroundCamera;
    public Canvas MenuCanvas;

    private Coroutine IntroCoroutine;

    private void Start()
    {
        MenuCanvas.worldCamera = Camera.main;
        IntroZoomImage = IntroZoomObject.GetComponent<Image>();
        IntroCoroutine = StartCoroutine(Play());
    }

    private void Update()
    {
        if (Input.touchCount > 0) // if touch input is used
        {
            StopCoroutine(IntroCoroutine);
            Destroy(gameObject);
        }
        else if (Input.GetMouseButton(0)) // if mouse click is used
        {
            StopCoroutine(IntroCoroutine);
            Destroy(gameObject);
        }
    }

    public IEnumerator Play()
    {
        // 1. Fade to black from splash screen colour
        Color StartingColor = new Color(0.1373f, 0.1216f, 0.1255f, 1);
        Color EndingColor = new Color(0, 0, 0, 1);
        AnimationBackground.color = StartingColor;
        BackgroundCamera.backgroundColor = StartingColor;

        float Duration = 0;
        float TotalDuration = 0.25f;

        while (Duration < TotalDuration) // Linear interpolate
        {
            AnimationBackground.color = Color.Lerp(StartingColor, EndingColor, Duration / TotalDuration);
            BackgroundCamera.backgroundColor = Color.Lerp(StartingColor, EndingColor, Duration / TotalDuration);
            Duration += Time.deltaTime;
            yield return null;
        }

        AnimationBackground.color = EndingColor;
        BackgroundCamera.backgroundColor = EndingColor;

        // 2. Show the intro sequence comic items.

        for (int i = 0; i < IntroSequenceItems.Length; i++)
        {
            IntroSequenceItems[i].SetActive(true);
            yield return new WaitForSeconds(1.833f); // wait for animation to complete.
        }

        // 3. Fade to the zoomed out main menu screen (by alpha adjustments).
        IntroZoomImage.color = new Color(1, 1, 1, 0.0f);
        IntroZoomObject.SetActive(true);

        Duration = 0;
        TotalDuration = 0.5f;

        while (Duration < TotalDuration)
        {
            IntroZoomImage.color = new Color(1, 1, 1, Duration / TotalDuration);
            Duration += Time.deltaTime;
            yield return null;
        }

        IntroZoomImage.color = new Color(1, 1, 1, 1.0f);

        // 4. Hide the intro sequence comic items.

        for (int i = 0; i < IntroSequenceItems.Length; i++)
        {
            IntroSequenceItems[i].SetActive(false);
        }

        // 5. Wait before zooming into the main menu to allow art appreciation.

        yield return new WaitForSeconds(1);

        // 6. Zoom into the main menu screen.

        float StartZoom = 1.0f;
        float EndZoom = 2.1f; // Stores by how much the zoomed out version of the main menu is zoomed out.

        Duration = 0;
        TotalDuration = 1.833f; // Animates the same duration as the comic items.

        while (Duration < TotalDuration) // Use a circular interpolation to more smoothly zoom in.
        {
            float SmoothStepZoomValue = Mathf.SmoothStep(StartZoom, EndZoom, Duration / TotalDuration);
            IntroZoomObject.transform.localScale = new Vector3(SmoothStepZoomValue, SmoothStepZoomValue, IntroZoomObject.transform.localScale.z);
            float SmoothStepPosition_X_Value = Mathf.SmoothStep(0, -187, Duration / TotalDuration);
            float SmoothStepPosition_Y_Value = Mathf.SmoothStep(0, 593, Duration / TotalDuration);
            IntroZoomObject.transform.localPosition = new Vector2(SmoothStepPosition_X_Value, SmoothStepPosition_Y_Value);
            Duration += Time.deltaTime;
            yield return null;
        }

        IntroZoomObject.transform.localScale = new Vector3(EndZoom, EndZoom, EndZoom);
        IntroZoomObject.transform.localPosition = new Vector2(-187, 593);

        AnimationBackground.color = new Color(0, 0, 0, 0);

        Duration = 0;
        TotalDuration = 0.5f;
        while (Duration < TotalDuration)
        {
            IntroZoomImage.color = new Color(1, 1, 1, 1.0f - (Duration / TotalDuration));
            Duration += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
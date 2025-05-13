using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroSequenceManager : MonoBehaviour
{
    public GameObject[] IntroSequenceItems = new GameObject[8];

    public GameObject IntroZoomObject;
    private Image IntroZoomImage;
    private Image AnimationBackground;
    public Camera BackgroundCamera;

    public bool AnimationSequencePlaying = true;

    private void Start()
    {
        IntroZoomImage = IntroZoomObject.GetComponent<Image>();
        AnimationBackground = GetComponent<Image>();
    }

    public IEnumerator Play()
    {
        AnimationSequencePlaying = true;

        Color StartingColor = new Color(0.1373f, 0.1216f, 0.1255f, 1);
        Color EndingColor = new Color(0, 0, 0, 1);
        AnimationBackground.color = StartingColor;
        BackgroundCamera.backgroundColor = StartingColor;

        float Duration = 0;
        float TotalDuration = 0.25f;

        while (Duration < TotalDuration)
        {
            AnimationBackground.color = Color.Lerp(StartingColor, EndingColor, Duration / TotalDuration);
            BackgroundCamera.backgroundColor = Color.Lerp(StartingColor, EndingColor, Duration / TotalDuration);
            Duration += Time.deltaTime;
            yield return null;
        }

        AnimationBackground.color = EndingColor;
        BackgroundCamera.backgroundColor = EndingColor;

        for (int i = 0; i < IntroSequenceItems.Length; i++)
        {
            IntroSequenceItems[i].SetActive(true);
            yield return new WaitForSeconds(1.833f);
        }

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

        for (int i = 0; i < IntroSequenceItems.Length; i++)
        {
            IntroSequenceItems[i].SetActive(false);
        }

        yield return new WaitForSeconds(1);

        float StartZoom = 1.0f;
        float EndZoom = 2.1f;

        Duration = 0;
        TotalDuration = 1.833f;

        while (Duration < TotalDuration)
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

        AnimationSequencePlaying = false;
    }
}
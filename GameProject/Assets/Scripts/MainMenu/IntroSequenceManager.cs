using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroSequenceManager : MonoBehaviour
{
    public GameObject[] IntroSequenceItems = new GameObject[8];

    public GameObject IntroZoomObject;
    private Image IntroZoomImage;

    public bool AnimationSequencePlaying = true;

    private void Start()
    {
        IntroZoomImage = IntroZoomObject.GetComponent<Image>();
    }

    public IEnumerator Play()
    {
        AnimationSequencePlaying = true;

        for (int i = 0; i < IntroSequenceItems.Length; i++)
        {
            IntroSequenceItems[i].SetActive(true);
            yield return new WaitForSeconds(1.833f);
        }

        IntroZoomImage.color = new Color(1, 1, 1, 0.0f);
        IntroZoomObject.SetActive(true);

        float Duration = 0;
        float TotalDuration = 0.5f;

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
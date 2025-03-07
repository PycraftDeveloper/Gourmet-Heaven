using TMPro;
using UnityEngine;
using UnityEngine.AdaptivePerformance;

public class FPSCounterScript : MonoBehaviour
{
    public TextMeshProUGUI FPS_Counter;
    private float timer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Application.targetFrameRate = -1;
    }

    // Update is called once per frame
    private void Update()
    {
        string target_fps;
        if (Application.targetFrameRate == -1)
        {
            target_fps = "(vsync, " + Screen.currentResolution.refreshRateRatio.value.ToString() + ")";
        }
        else
        {
            target_fps = Application.targetFrameRate.ToString();
        }

        timer += Time.deltaTime;
        if (timer > 0.5)
        {
            FPS_Counter.text = " FPS: " + (Mathf.Round(1 / Time.deltaTime)).ToString() + ", Target FPS: " + target_fps;
            timer = 0;
        }
    }
}
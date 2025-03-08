using TMPro;
using UnityEngine;

public class FPSCounterScript : MonoBehaviour
{
    public TextMeshProUGUI FPS_Counter;
    private float timer = 0;
    private int update = 0;

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
            FPS_Counter.text = " FPS: " + (Mathf.Round(1 / Time.deltaTime)).ToString() + ", Target FPS: " + target_fps + ", Update No.: " + update.ToString();
            timer = 0;
            update++;
        }
    }
}
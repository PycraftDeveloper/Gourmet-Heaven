// The following program was written by Emmie Heane.

using UnityEngine;
using TMPro;


public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    public TextMeshProUGUI minigamefailedText;

    [SerializeField] private float remainingTime;
    public float delayTime = 2f;

    public GameObject SplashArtFail;
    
    private bool isRunning = true;

    private void ReturnToKitchen()
    {
        Registry.GameManagerObject.ChangeScene();
    }

    private void Update()
    {
        if (!isRunning) return;

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            minigamefailedText.gameObject.SetActive(false);
        }
        else if (remainingTime <= 0) // Mini-game failed condition
        {
           SplashArtFail.gameObject.SetActive(true);
            remainingTime = 0;
            timerText.color = Color.red;
            minigamefailedText.gameObject.SetActive(true);
            Invoke("ReturnToKitchen", 2f);
            StopTimer(); // Force the timer to stop this from being called multiple times.
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    public void StopTimer()
    {
        isRunning = false;
        enabled = false;
    }
}
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float remainingTime;
    public TextMeshProUGUI minigamefailedText;
    public float delayTime = 2f;
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
            remainingTime = 0;
            timerText.color = Color.red;
            minigamefailedText.gameObject.SetActive(true);
            Invoke("ReturnToKitchen", 4f);
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
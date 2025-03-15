using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    public TextMeshProUGUI minigamefailedText;
    public float delayTime = 2f;

    void ReturnToResturant()
    {
        SceneManager.LoadScene("Restaurant"); // can be modified later, need to add a delay before opening the restaurant scene
    }

    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            minigamefailedText.gameObject.SetActive(false);
        } 
        else if (remainingTime < 0) // mini game failed condition 
        {
            remainingTime = 0;
            timerText.color = Color.red;
            minigamefailedText.gameObject.SetActive(true);
            Invoke("ReturnToResturant", 2.0f);
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

}

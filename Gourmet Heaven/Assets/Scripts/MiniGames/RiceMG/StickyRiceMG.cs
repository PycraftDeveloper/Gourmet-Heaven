using UnityEngine;
using TMPro;

public class SlicedObject : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI feedbackText;

    public GameObject arrowPrefab;
    public GameObject arrowHeadPrefab;
    [HideInInspector] public GameObject currentArrow;
    [HideInInspector] public GameObject currentArrowHead;
    public GameObject SlicedRice;
    public GameObject WholeRice;
    public GameObject CutRice;
    public GameObject MiniGameTimerObject;
    private CountdownTimer MiniGameTimer;
    public Canvas MenuCanvas;
    [SerializeField] private CountdownTimer countdowntimer;

    [Header("Slice control")]
    public Transform[] slicePoints;

    private Vector2[] randomDirections;
    private bool[] pointsSliced;
    private bool isSliced = false;
    private bool MiniGameOverLock = false;
    private int currentSliceIndex = 0;

    [Header("End menus")]
    public GameObject SuccessSplashArt;

    [Header("Music")]
    public AudioClip BackgroundMusic;

    [Header("SFX")]
    public OneShotSetup MangoFinish;

    private void Start()
    {
        Registry.CGI.MiniGameMusicSource.clip = BackgroundMusic;
        Registry.CGI.MiniGameMusicSource.loop = false;
        Registry.CGI.MiniGameMusicSource.Play();

        Registry.CGI.GameMusicSource.Pause();
        Registry.CGI.RestaurantAmbienceSource.Pause();
        Registry.InMiniGame = true;

        MenuCanvas.worldCamera = Camera.main;
        MenuCanvas.sortingLayerName = "UI";

        MiniGameTimer = MiniGameTimerObject.GetComponent<CountdownTimer>();

        StartMinigame();
    }

    public void StartMinigame()
    {
        MiniGameTimer.isRunning = true;

        CountdownTimer countdownTimer = Object.FindFirstObjectByType<CountdownTimer>();

        pointsSliced = new bool[slicePoints.Length];
        randomDirections = new Vector2[slicePoints.Length];

        if (feedbackText != null)
            feedbackText.gameObject.SetActive(false);

        ShuffleSlicePoints();
        RandomizeSliceDirections();
        CreateArrow(slicePoints[0]);
    }

    private void ShuffleSlicePoints()
    {
        for (int i = 0; i < slicePoints.Length; i++)
        {
            Transform temp = slicePoints[i];
            int randomIndex = Random.Range(i, slicePoints.Length);
            slicePoints[i] = slicePoints[randomIndex];
            slicePoints[randomIndex] = temp;
        }
    }

    private void RandomizeSliceDirections()
    {
        for (int i = 0; i < slicePoints.Length - 1; i++)
        {
            Vector2 direction = slicePoints[i + 1].position - slicePoints[i].position;
            randomDirections[i] = direction.normalized;
        }
    }

    private void CreateArrow(Transform currentPoint)
    {
        if (currentArrow != null)
        {
            Destroy(currentArrow);
            Destroy(currentArrowHead); // (TJ)
        }

        if (currentSliceIndex >= slicePoints.Length - 1) return;

        Vector3 start = slicePoints[currentSliceIndex].position;
        start.z = 0; // Ensure the arrow is on the correct plane
        Vector3 end = slicePoints[currentSliceIndex + 1].position;
        end.z = 0; // Ensure the arrow is on the correct plane
        Vector3 direction = end - start;

        // Instantiate arrow at the middle between the two points
        Vector3 midPoint = (start + end) / 2f;

        currentArrow = Instantiate(arrowPrefab, midPoint, Quaternion.identity);
        currentArrowHead = Instantiate(arrowHeadPrefab, end, Quaternion.identity); // (TJ)

        // Rotate arrow to match direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        currentArrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        currentArrowHead.transform.rotation = Quaternion.Euler(0, 0, angle + 180); // (TJ)

        // Scale arrow based on distance (make sure arrow prefab's default length is 1 unit wide)
        float distance = direction.magnitude;
        currentArrow.transform.localScale = new Vector3(distance, currentArrow.transform.localScale.y, 1f);
    }

    // checking to see if the direction of the slice is correct and if the player hits both of the slice points, shows the feedback text to indicate if they failed or successed
    public void TrySlice(Vector2 swipeStart, Vector2 swipeEnd)
    {
        if (isSliced) return;

        if (slicePoints.Length < 2 || pointsSliced[currentSliceIndex])
        {
            return;
        }

        if (IsCorrectSliceDirection(swipeStart, swipeEnd))
        {
            ShowMessage("Well Done!");
            SliceObject();
        }
        else
        {
            ShowMessage("Try Again!");
        }
    }

    private bool IsCorrectSliceDirection(Vector2 start, Vector2 end)
    {
        Vector2 correctDirection = randomDirections[currentSliceIndex];
        Vector2 swipeDirection = (end - start).normalized;
        float dotProduct = Vector2.Dot(swipeDirection, correctDirection);

        float angleTolerance = 10f;
        float angle = Vector2.Angle(swipeDirection, correctDirection);
        return angle < angleTolerance || dotProduct > 0.7f;
    }

    private void ShowMessage(string message)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            feedbackText.gameObject.SetActive(true);
            Invoke("HideMessage", 1.5f);
        }
    }

    private void HideMessage()
    {
        if (feedbackText != null)
            feedbackText.gameObject.SetActive(false);
    }

    private void SliceObject()
    {
        pointsSliced[currentSliceIndex] = true;

        if (currentSliceIndex >= slicePoints.Length - 2)
        {
            isSliced = true;
            Registry.CGI.PlayExtendedOneShot(MangoFinish);
            ShowMessage("Amazing!");

            Registry.PlayerObject.HoldingMeal = Constants.MANGO_STICKY_RICE;

            Registry.RiceMGTutorialShown = true;

            if (countdowntimer != null)
            {
                countdowntimer.StopTimer();
            }

            SwapToSliceAsset();
        }
        else
        {
            currentSliceIndex++;
            CreateArrow(slicePoints[currentSliceIndex]);
        }
    }

    private void SwapToSliceAsset()
    {
        if (currentArrow != null)
        {
            Destroy(currentArrow);
            Destroy(currentArrowHead);
        }

        if (WholeRice != null)
        {
            WholeRice.SetActive(false);
        }

        if (CutRice != null)
        {
            CutRice.SetActive(true);
        }

        if (MiniGameOverLock == false)
        {
            MiniGameOverLock = true;
            Invoke("ShowSplashArt", 2f);
        }
    }

    private void ShowSplashArt()
    {
        Destroy(currentArrow);
        Destroy(currentArrowHead);

        SuccessSplashArt.gameObject.SetActive(true);

        Registry.CGI.GameMusicSource.UnPause();
        Registry.CGI.RestaurantAmbienceSource.UnPause();
        Registry.CGI.MiniGameMusicSource.Stop();

        Invoke("ReturnToKitchen", 2f);
    }

    private void ReturnToKitchen()
    {
        Registry.InMiniGame = false;
        Destroy(this.gameObject);
    }
}
// The following program was written by Emmie Heane.

using UnityEngine;
using TMPro;

public class SlicedObject : MonoBehaviour
{
    public TextMeshProUGUI feedbackText; // feedback text to tell the player if they are doing the swipe right or wrong

    public Transform[] slicePoints; // the slice points to indicate where the player needs to cut
    private Vector2[] randomDirections;// to make the cut direction random every time the game is played

    private bool[] pointsSliced; // to keep track of how many cuts the player has done
    private bool isSliced = false; // so the object doesnt slice when playing the game until all cuts have been done

    private int currentSliceIndex = 0; // to keep track of how many cuts needs to be done

    public GameObject arrowPrefab; // allows the arrow prefab to work and shows where the player needs to cut
    private GameObject currentArrow; // shows where the player needs to cut
    public GameObject SlicedRice;
    public GameObject WholeRice;
    public GameObject CutRice;

    [SerializeField] private CountdownTimer countdowntimer;

    private void Start()
    {
        CountdownTimer countdownTimer = Object.FindFirstObjectByType<CountdownTimer>();

        // creates the random direction the player needs to slice and hides the feedback text until the player trys to do a swipe
        pointsSliced = new bool[slicePoints.Length];
        randomDirections = new Vector2[slicePoints.Length];

        if (feedbackText != null)
            feedbackText.gameObject.SetActive(false);
        //creates the arrow prefab to show where the player needs to slice
        ShuffleSlicePoints();
        RandomizeSliceDirections();
        CreateArrow(slicePoints[0]);
    }

    // shuffles the slice points every time the game is played
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

    // randomizes the direction where the player needs to slice everytime the game is played
    private void RandomizeSliceDirections()
    {
        for (int i = 0; i < slicePoints.Length - 1; i++)
        {
            Vector2 direction = slicePoints[i + 1].position - slicePoints[i].position;
            randomDirections[i] = direction.normalized;
        }
    }

    // creates and destroys the arrow prefab when the player does a successful slice
    private void CreateArrow(Transform currentPoint)
    {
        if (currentArrow != null)
        {
            Destroy(currentArrow);
        }

        if (currentSliceIndex >= slicePoints.Length - 1) return;

        Vector3 start = slicePoints[currentSliceIndex].position;
        Vector3 end = slicePoints[currentSliceIndex + 1].position;
        Vector3 direction = end - start;

        // Instantiate arrow at the middle between the two points
        Vector3 midPoint = (start + end) / 2f;
        currentArrow = Instantiate(arrowPrefab, midPoint, Quaternion.identity);

        // Rotate arrow to match direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        currentArrow.transform.rotation = Quaternion.Euler(0, 0, angle);

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
            Debug.LogError("Invalid slice points!");
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

    // setting up the correct slice direction
    private bool IsCorrectSliceDirection(Vector2 start, Vector2 end)
    {
        Vector2 correctDirection = randomDirections[currentSliceIndex];
        Vector2 swipeDirection = (end - start).normalized;
        float dotProduct = Vector2.Dot(swipeDirection, correctDirection);

        float angleTolerance = 10f;
        float angle = Vector2.Angle(swipeDirection, correctDirection);
        return angle < angleTolerance || dotProduct > 0.7f;
    }

    // sets the feedback text message, colour and visibility
    private void ShowMessage(string message)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            feedbackText.gameObject.SetActive(true);
            Invoke("HideMessage", 1.5f);
        }
    }

    // hides the feedback text
    private void HideMessage()
    {
        if (feedbackText != null)
            feedbackText.gameObject.SetActive(false);
    }

    // checking to see if all of the slice points have been done and then slicing the object
    private void SliceObject()
    {
        pointsSliced[currentSliceIndex] = true;

        if (currentSliceIndex >= slicePoints.Length - 2)
        {
            isSliced = true;
            Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.audioClip3);
            ShowMessage("Amazing!");
            Debug.Log("Mango Fully Sliced!");
            Registry.PlayerObject.GetComponent<Player>().HoldingMeal = Constants.MANGO_STICKY_RICE;

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

    private void ReturnToKitchen()
    {
        Registry.GameManagerObject.ChangeScene();
    }

    // swaps to the sliced asset
    private void SwapToSliceAsset()
    {
        if (currentArrow != null)
        {
            Destroy(currentArrow);
        }

        // Disable the whole rice asset
        if (WholeRice != null)
        {
            WholeRice.SetActive(false);
        }

        // enable the cut asset
        if (CutRice != null)
        {
            CutRice.SetActive(true);
        }

        // Return to kitchen after short delay
        Invoke("ReturnToKitchen", 4f);
    }
}
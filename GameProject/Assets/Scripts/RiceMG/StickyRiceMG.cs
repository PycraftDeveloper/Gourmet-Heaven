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

    public Vector3 spriteScale = new Vector3(1.5f, 1.5f, 1); // setting the sizes cut peices (will be changed to create another object, the fully sliced asset)

    public Color pieceColor = Color.red; // setting the colour of the pieces (will be deleted once i have the fully sliced asset)

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
            int randomIndex = Random.Range(i + 1, slicePoints.Length);
            Vector2 direction = slicePoints[randomIndex].position - slicePoints[i].position;
            randomDirections[i] = direction.normalized;
        }
    }

    // creates and destroys the arrow prefab when the player does a successful slice
    private void CreateArrow(Transform slicePoint)
    {
        if (currentArrow != null)
        {
            Destroy(currentArrow);
        }
        currentArrow = Instantiate(arrowPrefab, slicePoint.position, Quaternion.identity);
        currentArrow.transform.up = randomDirections[currentSliceIndex];
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
            ShowMessage("Well Done!", Color.green);
            SliceObject();
        }
        else
        {
            ShowMessage("Try Again!", Color.red);
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
    private void ShowMessage(string message, Color color)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            feedbackText.color = color;
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

        if (currentSliceIndex >= slicePoints.Length - 1)
        {
            isSliced = true;
            Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.audioClip3);
            ShowMessage("Amazing!", Color.green);
            Debug.Log("Mango Fully Sliced!");
            Registry.PlayerObject.GetComponent<Player>().HoldingMeal = Constants.MANGO_STICKY_RICE;

            if (countdowntimer != null)
            {
                countdowntimer.StopTimer();
            }

            CreateSlicedPieces();
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

    // creates the sliced pieces (will be deleted after)
    private void CreateSlicedPieces()
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject piece = new GameObject("SlicedPiece_" + i);
            SpriteRenderer spriteRenderer = piece.AddComponent<SpriteRenderer>();

            // create a simple object when the object is fully sliced (placeholder)
            Texture2D texture = new Texture2D(100, 100);
            for (int y = 0; y < 100; y++)
            {
                for (int x = 0; x < 100; x++)
                {
                    texture.SetPixel(x, y, pieceColor);
                }
            }
            texture.Apply();

            spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            spriteRenderer.sortingOrder = 5; // makes sure that the pieces renders on top of everything else(will be deleted)

            piece.transform.position = transform.position + new Vector3(i * 0.3f - 0.8f, Random.Range(-0.2f, 0.2f), 0);
            piece.transform.localScale = spriteScale;

            // adds gravity to the pieces (will be deleted)
            Rigidbody2D rb = piece.AddComponent<Rigidbody2D>();
            BoxCollider2D boxCollider = piece.AddComponent<BoxCollider2D>();

            rb.gravityScale = 1f;
            rb.AddForce(new Vector2(Random.Range(-2f, 2f), Random.Range(2f, 5f)), ForceMode2D.Impulse);
        }

        // removes the arrow after slicing
        if (currentArrow != null)
        {
            Destroy(currentArrow);
        }

        // destroys the original object after creating the sliced asset
        Destroy(SlicedRice);
        Invoke("ReturnToKitchen", 4f);
    }
}
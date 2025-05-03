using UnityEngine;

public class IngredientSpawn : MonoBehaviour
{
    public GameObject IngredientPrefab;
    private GameObject SpawnedIngredientObject;
    private Ingredient SpawnedIngredient;

    public GameObject MiniGameManagerObject;
    private SushiMiniGameManager SushiMiniGameManager;

    private bool IngredientSpawned = false;
    public bool IngredientDraggedIntoTargetToggle = false;

    private void Start()
    {
        SushiMiniGameManager = MiniGameManagerObject.GetComponent<SushiMiniGameManager>();
    }

    private void HandleTouch(Vector2 TouchPosition)
    {
        Vector2 WorldPosition = Camera.main.ScreenToWorldPoint(TouchPosition);
        Collider2D RaycastHit = Physics2D.OverlapPoint(WorldPosition);

        if (RaycastHit != null && RaycastHit.transform == transform && !SushiMiniGameManager.IngredientSpawned)
        {
            SpawnedIngredientObject = Instantiate(IngredientPrefab, WorldPosition, transform.rotation);
            SpawnedIngredient = SpawnedIngredientObject.GetComponent<Ingredient>();
            SpawnedIngredient.SetPosition(TouchPosition);
            SushiMiniGameManager.IngredientSpawned = true;
            IngredientSpawned = true;
        }
    }

    private void Update()
    {
        if (SpawnedIngredient == null)
        {
            if (Input.touchCount > 0) // if touch input is used
            {
                UnityEngine.Touch touch = Input.GetTouch(0); // get the first finger (only guaranteed input)

                HandleTouch(touch.position); // handle position adjustments, using finger position on-screen.
            }
            else if (Input.GetMouseButton(0)) // if mouse click is used
            {
                HandleTouch(Input.mousePosition); // handle position adjustments, using mouse position.
            }
        }
        else
        {
            if (Input.touchCount > 0) // if touch input is used
            {
                UnityEngine.Touch touch = Input.GetTouch(0); // get the first finger (only guaranteed input)

                SpawnedIngredient.SetPosition(touch.position);
            }
            else if (Input.GetMouseButton(0)) // if mouse click is used
            {
                SpawnedIngredient.SetPosition(Input.mousePosition);
            }
            else
            {
                if (SpawnedIngredient.InTarget)
                {
                    IngredientDraggedIntoTargetToggle = true;
                }

                SpawnedIngredient = null;
                Destroy(SpawnedIngredientObject);

                if (IngredientSpawned)
                {
                    SushiMiniGameManager.IngredientSpawned = false;
                    IngredientSpawned = false;
                }
            }
        }
    }
}
using UnityEngine;

public class SushiMain : MonoBehaviour
{
    private string[] IngredientOrder = new string[3];
    private string[] TargetOrder = new string[3] { Constants.SEAWEED, Constants.RICE, Constants.SOMETHING_ELSE };

    private int IngredientIndex = 0;

    public void OnTriggerEnter2D(Collider2D Collision)
    {
    }

    public void OnTriggerExit2D(Collider2D Collision)
    {
        DragDrop CollisionDragDrop = Collision.GetComponent<DragDrop>();
        if (CollisionDragDrop.IsMovable == false)
        {
            IngredientOrder[IngredientIndex] = Collision.name;
            IngredientIndex++;
            CollisionDragDrop.Reuse();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        bool MealCorrect = true;
        for (int i = 0; i < IngredientIndex; i++)
        {
            if (IngredientOrder[i] != TargetOrder[i])
            {
                MealCorrect = false;
                break;
            }
        }

        if (IngredientIndex == 3)
        {
            if (MealCorrect)
            {
                Debug.Log("Mini Game Won!");
                Registry.PlayerObject.GetComponent<Player>().HoldingMeal = Constants.SUSHI;
            }
            else
            {
                Debug.Log("Mini Game Failed!");
                // Mini-game not completed correctly
            }
            Registry.GameManagerObject.ChangeScene();
        }
    }
}
using TMPro;
using UnityEngine;

public class EndMenuManagerScript : MonoBehaviour
{
    private int StoredPlayerScore = Registry.PlayerScore;
    private int StoredMaximumScore = Registry.MaxScore;

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI GradeText;

    public GameObject[] StarScores = new GameObject[5];

    public void Start()
    {
        Debug.Log("Don't forget about me when balancing the game!");

        for (int i = 0; i < Registry.Customers.Count; i++)
        {
            Customer thisCustomer = Registry.Customers[i].GetComponent<Customer>();
            if (thisCustomer != null)
            {
                if (thisCustomer.Meal != "")
                {
                    StoredMaximumScore += 100;
                }
            }
        }

        if (StoredPlayerScore > StoredMaximumScore)
        {
            StoredMaximumScore = StoredPlayerScore;
        }

        ScoreText.text = StoredPlayerScore.ToString() + " / " + StoredMaximumScore.ToString();

        if (StoredPlayerScore / StoredMaximumScore > 0.9) // can easily add more, go from largest to smallest
        {
            GradeText.text = "Perfect";
        }
        else if (StoredPlayerScore / StoredMaximumScore > 0.5) // use '_' instead of commas for large values, eg: 9_000 or 1_000_000.
        {
            GradeText.text = "Okay";
        } // add more here if needed
        else
        {
            GradeText.text = "Bad";
        }

        int StarScoreID = ((int)((StoredPlayerScore / (float)(StoredMaximumScore + 1)) * 5)); // using 'max score' +1 to ensure index always in bounds.
        Instantiate(StarScores[StarScoreID], transform);
    }

    public void OnPlayAgainButtonClick()
    {
        Registry.GameManagerObject.ResetGameLevel();
        if (Registry.LevelNumber == Constants.LEVEL_ONE)
        {
            Registry.LevelCustomiserObject.SetupLevelOne();
        }
        else
        {
            Registry.LevelCustomiserObject.SetupLevelTwo();
        }
        Registry.GameManagerObject.ChangeScene(Constants.KITCHEN);
    }

    public void OnMainMenuButtonClick()
    {
        Registry.GameManagerObject.ChangeScene(Constants.MAIN_MENU);
    }

    public void OnQuitButtonClick()
    {
        Registry.GameManagerObject.QuitGame();
    }
}
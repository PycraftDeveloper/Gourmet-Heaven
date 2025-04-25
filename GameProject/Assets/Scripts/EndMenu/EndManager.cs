using TMPro;
using UnityEngine;

public class EndMenuManagerScript : MonoBehaviour
{
    private int StoredPlayerScore = Registry.PlayerScore;

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI GradeText;

    public GameObject[] StarScores = new GameObject[5];

    public void Start()
    {
        Debug.Log("Don't forget about me when balancing the game!");

        ScoreText.text = StoredPlayerScore.ToString() + " / 1000";

        if (StoredPlayerScore > 900) // can easily add more, go from largest to smallest
        {
            GradeText.text = "Perfect";
        }
        else if (StoredPlayerScore > 500) // use '_' instead of commas for large values, eg: 9_000 or 1_000_000.
        {
            GradeText.text = "Okay";
        } // add more here if needed
        else
        {
            GradeText.text = "Bad";
        }

        int StarScoreID = ((int)((StoredPlayerScore / 1001.0f) * 5)); // using 'max score' +1 to ensure index always in bounds.
        Instantiate(StarScores[StarScoreID], transform);
    }

    public void OnPlayAgainButtonClick()
    {
        Registry.GameManagerObject.ResetGameLevel();
        if (Registry.LevelNumber == Constants.LEVEL_ONE)
        {
            Registry.LevelCustomerObject.SetupLevelOne();
        }
        else
        {
            Registry.LevelCustomerObject.SetupLevelTwo();
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
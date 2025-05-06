using TMPro;
using UnityEngine;

public class EndMenuManagerScript : MonoBehaviour
{
    private int StoredPlayerScore = Registry.PlayerScore;
    private int StoredMaxScore = 0;

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI GradeText;

    public GameObject[] StarScores = new GameObject[5];

    public void Start()
    {
        StoredMaxScore = Constants.LEVEL_ONE_MAX_SCORE;
        if (Registry.LevelNumber == Constants.LEVEL_TWO)
        {
            StoredMaxScore = Constants.LEVEL_TWO_MAX_SCORE;
        }

        ScoreText.text = StoredPlayerScore.ToString() + " / " + StoredMaxScore.ToString();

        if (StoredPlayerScore / StoredMaxScore > 0.9) // can easily add more, go from largest to smallest
        {
            GradeText.text = "Perfect";
        }
        else if (StoredPlayerScore / StoredMaxScore > 0.5) // use '_' instead of commas for large values, eg: 9_000 or 1_000_000.
        {
            GradeText.text = "Okay";
        } // add more here if needed
        else
        {
            GradeText.text = "Bad";
        }

        int StarScoreID = ((int)((StoredPlayerScore / (float)(StoredMaxScore + 1)) * 5)); // using 'max score' +1 to ensure index always in bounds.
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
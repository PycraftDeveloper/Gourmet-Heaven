using TMPro;
using UnityEngine;

public class EndMenuManagerScript : MonoBehaviour
{
    private int StoredPlayerScore = Registry.PlayerScore; // Stores the player's score before it is reset.
    private float StoredMaxScore = 0;

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI GradeText;

    public GameObject[] StarScores = new GameObject[5]; // Stores the prefabs for the individual star arrangements for the 1, 2, 3, 4 and 5 star scores.

    public void Start()
    {
        // Determine the max score based on the level.
        StoredMaxScore = Constants.LEVEL_ONE_MAX_SCORE;
        if (Registry.LevelNumber == Constants.LEVEL_TWO)
        {
            StoredMaxScore = Constants.LEVEL_TWO_MAX_SCORE;
        }

        ScoreText.text = StoredPlayerScore.ToString() + " / " + StoredMaxScore.ToString(); // Display the player's score and max score.

        if (StoredPlayerScore / StoredMaxScore > 0.9) // can easily add more, go from largest to smallest
        {
            GradeText.text = "Perfect"; // if the player has achieved more than 90% of the max score, they get a perfect grade.
        }
        else if (StoredPlayerScore / StoredMaxScore > 0.5) // use '_' instead of commas for large values, eg: 9_000 or 1_000_000.
        {
            GradeText.text = "Okay"; // if the player has achieved more than 50% of the max score, they get an okay grade.
        } // add more here if needed
        else
        {
            GradeText.text = "Bad"; // otherwise the player is graded as bad.
        }

        int StarScoreID = ((int)((StoredPlayerScore / (float)(StoredMaxScore + 1)) * 5)); // using 'max score' +1 to ensure index always in bounds.
        StarScoreID = Mathf.Clamp(StarScoreID, 0, StarScores.Length - 1); // Clamp the score to the range of 0 to 4, to account for current score being allowed to be higher than max score.
        Instantiate(StarScores[StarScoreID], transform); // Display the player's number of stars awarded for the game.

        Registry.GameTutorialShown = true; // Set the game tutorial to be shown, so it doesn't show again when the player goes back to the level selection menu.
    }

    public void OnPlayAgainButtonClick() // Used to restart the game by going back to the level the player was previously playing.
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
        Registry.GameManagerObject.ChangeScene(Constants.GAME_LEVEL);
    }

    // The remainder of these functions are not used, but would allow the player to go back to the main menu or quit the game from the end menu.
    public void OnMainMenuButtonClick()
    {
        Registry.GameManagerObject.ChangeScene(Constants.MAIN_MENU);
    }

    public void OnQuitButtonClick()
    {
        Registry.GameManagerObject.QuitGame();
    }
}
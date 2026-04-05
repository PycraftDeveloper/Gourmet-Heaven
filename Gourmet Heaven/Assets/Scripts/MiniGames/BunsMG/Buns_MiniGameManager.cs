using System.Collections;
using UnityEngine;

public class Buns_MiniGameManager : MonoBehaviour
{
    // Store all the content that the mini-game needs to manage from the hierarchy.
    public GameObject HandPointerObject;

    public GameObject FireIndicatorObject;
    public GameObject EggTimerObject;
    public GameObject HobKnobObject;

    public GameObject MiniGameFailedPopUp;
    public GameObject MiniGameWinPopUp;

    public Canvas MenuCanvas;

    private Animator EggTimerAnimator; // Store the animator for the egg timer object.

    private float CurrentMiniGameDuration = 0; // Store how long the mini-game has been running for.
    private float TargetMiniGameDuration; // Stores the random amount of time the player needs to wait for (between 5 and 15 seconds).

    private bool MiniGameLocked = false;

    private bool IsHobOn = true; // Used to determine if the mini-game has been won or not.

    private Vector3 EggTimerPosition = new Vector3(-3.53f, -0.1f, 100); // Store the position of the egg timer object on-screen.

    [Header("Music")]
    public AudioClip BackgroundMusic;

    [Header("SFX")]
    public AudioClip BoilingWater;

    public OneShotSetup EggTimerTicking;
    public OneShotSetup EggTimerAlarm;
    private GameObject EggTimerTickingExtendedOneShotObject;

    private void ReturnToKitchen()
    {
        Registry.InMiniGame = false;
        Destroy(this.gameObject);
    }

    private void OnMiniGameFailed()
    {
        MiniGameLocked = true;
        MiniGameFailedPopUp.SetActive(true);
        Registry.CGI.GameMusicSource.UnPause();
        Registry.CGI.RestaurantAmbienceSource.UnPause();
        Registry.CGI.MiniGameMusicSource.Stop();
        if (EggTimerTickingExtendedOneShotObject != null)
        {
            Destroy(EggTimerTickingExtendedOneShotObject);
        }
        Invoke("ReturnToKitchen", 2);
    }

    private void ShowMiniGameSucsess()
    {
        MiniGameWinPopUp.SetActive(true);
        Registry.PlayerObject.HoldingMeal = Constants.BAO_BUNS;
        Registry.CGI.GameMusicSource.UnPause();
        Registry.CGI.RestaurantAmbienceSource.UnPause();
        Registry.CGI.MiniGameMusicSource.Stop();
        if (EggTimerTickingExtendedOneShotObject != null)
        {
            Destroy(EggTimerTickingExtendedOneShotObject);
        }
        Registry.BunsMGTutorialShown = true;
        Invoke("ReturnToKitchen", 2);
    }

    private void OnMiniGameWin()
    {
        MiniGameLocked = true;

        ShowMiniGameSucsess();
    }

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

        TargetMiniGameDuration = Random.Range(Constants.BUNS_TIME_DELAY[0], Constants.BUNS_TIME_DELAY[1]); // generate a random time the player needs to wait for.

        EggTimerAnimator = EggTimerObject.GetComponent<Animator>();

        if (!MiniGameLocked)
        {
            // Start of Joshua Cossar's Added Code
            Registry.CGI.SFXSource.clip = BoilingWater;
            Registry.CGI.SFXSource.volume = Registry.SFXVolume;
            Registry.CGI.SFXSource.Play();
            Registry.CGI.SFXSource.loop = true;
            EggTimerTickingExtendedOneShotObject = Registry.CGI.PlayExtendedOneShot(EggTimerTicking);
            // End of Joshua Cossar's Added Code
        }
    }

    public void HandleTouch()
    {
        IsHobOn = false;
        // Start of Joshua Cossar's Added Code
        Registry.CGI.SFXSource.Stop();
        // End of Joshua Cossar's Added Code
    }

    private IEnumerator RotateHobKnob(GameObject HobKnob) // Used to rotate the hob dial to the off-position when interacted with.
    {
        float Duration = 0.5f;
        float RunTime = 0;

        Transform HobKnobTransform = HobKnob.transform;

        while (RunTime < Duration) // Linearly interpolate a rotation value.
        {
            HobKnobTransform.rotation = Quaternion.Euler(0, 0, (-90) + (90 * (RunTime / Duration)));
            RunTime += Time.deltaTime;
            yield return null;
        }
    }

    private void Update()
    {
        if (!MiniGameLocked)
        {
            CurrentMiniGameDuration += Time.deltaTime; // Count how long the mini-game has been running for.

            if (CurrentMiniGameDuration > TargetMiniGameDuration) // If the egg timer has gone off
            {
                if (!HandPointerObject.activeSelf) // Show the hand pointer so the player knows where to click.
                {
                    HandPointerObject.SetActive(true);

                    EggTimerPosition.y += Constants.ACTIVE_EGG_TIMER_DISPLACEMENT; // Displace the egg timer to show the player it has gone off.

                    Registry.CGI.PlayExtendedOneShot(EggTimerAlarm); // Added by Joshua Cossar
                    EggTimerAnimator.SetBool("Alarm", true); // Added by Joshua Cossar
                }

                if (CurrentMiniGameDuration > TargetMiniGameDuration + Constants.BUNS_REACTION_THRESHOLD) // Determine if the player took too long to react.
                {
                    OnMiniGameFailed();
                }

                if (!IsHobOn) // When the hob dial has been turned off.
                {
                    // Hide the indicators - these are no longer needed.
                    HandPointerObject.SetActive(false);
                    FireIndicatorObject.SetActive(false);
                    MiniGameLocked = true;
                    StartCoroutine(RotateHobKnob(HobKnobObject)); // Display the hob dial rotating.
                    EggTimerAnimator.enabled = false;
                    EggTimerPosition.y -= Constants.ACTIVE_EGG_TIMER_DISPLACEMENT; // Reset the egg timer position.
                    Invoke("OnMiniGameWin", 2); // Display the mini-game win pop-up after a delay.
                }
            }
            else if (!IsHobOn)
            {
                OnMiniGameFailed(); // If the player turns the hob off prematurely, the mini-game is failed.
            }

            EggTimerObject.transform.position = EggTimerPosition; // Position the egg timer on-screen.
        }
    }
}
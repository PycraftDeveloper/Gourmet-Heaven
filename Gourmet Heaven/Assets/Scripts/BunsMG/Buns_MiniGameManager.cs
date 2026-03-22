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

    public AudioClip BackgroundMusic;

    private float CurrentMiniGameDuration = 0; // Store how long the mini-game has been running for.
    private float TargetMiniGameDuration; // Stores the random amount of time the player needs to wait for (between 5 and 15 seconds).

    private bool MiniGameLocked = false;

    private bool IsHobOn = true; // Used to determine if the mini-game has been won or not.

    private Vector3 EggTimerPosition = new Vector3(-3.53f, -0.1f, 100); // Store the position of the egg timer object on-screen.

    private void ReturnToKitchen()
    {
        Registry.InMiniGame = false;
        Registry.CoreGameInfrastructureObject.CloseMenu();
    }

    private void OnMiniGameFailed()
    {
        MiniGameLocked = true;
        MiniGameFailedPopUp.SetActive(true);
        Registry.CoreGameInfrastructureObject.GameMusicSource.UnPause();
        Registry.CoreGameInfrastructureObject.musicSource.Stop();
        Invoke("ReturnToKitchen", 2);
    }

    private void ShowMiniGameSucsess()
    {
        MiniGameWinPopUp.SetActive(true);
        Registry.PlayerObject.GetComponent<Player>().HoldingMeal = Constants.BAO_BUNS;
        Registry.CoreGameInfrastructureObject.GameMusicSource.UnPause();
        Registry.CoreGameInfrastructureObject.musicSource.Stop();
        Invoke("ReturnToKitchen", 2);
    }

    private void OnMiniGameWin()
    {
        MiniGameLocked = true;

        Registry.BunsMGTutorialShown = true;

        ShowMiniGameSucsess();
    }

    private void Start()
    {
        if (Registry.CoreGameInfrastructureObject.musicSource.clip != BackgroundMusic)
        {
            Registry.CoreGameInfrastructureObject.musicSource.clip = BackgroundMusic;
            Registry.CoreGameInfrastructureObject.musicSource.loop = false;
        }

        Registry.CoreGameInfrastructureObject.musicSource.Play();

        Registry.CoreGameInfrastructureObject.GameMusicSource.Pause();
        Registry.InMiniGame = true;

        MenuCanvas.worldCamera = Camera.main;
        MenuCanvas.sortingLayerName = "UI";

        TargetMiniGameDuration = Random.Range(Constants.BUNS_TIME_DELAY[0], Constants.BUNS_TIME_DELAY[1]); // generate a random time the player needs to wait for.

        EggTimerAnimator = EggTimerObject.GetComponent<Animator>();

        if (!MiniGameLocked)
        {
            // Start of Joshua Cossar's Added Code
            Registry.CoreGameInfrastructureObject.SFXSource.clip = Registry.CoreGameInfrastructureObject.BoilingWater;
            Registry.CoreGameInfrastructureObject.SFXSource.volume = Registry.SFXVolume;
            Registry.CoreGameInfrastructureObject.SFXSource.Play();
            Registry.CoreGameInfrastructureObject.SFXSource.loop = true;
            Registry.CoreGameInfrastructureObject.SFXSource.PlayOneShot(Registry.CoreGameInfrastructureObject.EggTimerTicking);
            // End of Joshua Cossar's Added Code
        }
    }

    private void HandleTouch(Vector2 TouchPosition) // Get the position on-screen the player interacted with, and determine if that interaction was intended to hit the oven
                                                    // dial to turn the hob off.
    {
        Vector2 WorldPosition = Camera.main.ScreenToWorldPoint(TouchPosition);
        Collider2D RaycastHit = Physics2D.OverlapPoint(WorldPosition);

        if (RaycastHit != null && RaycastHit.transform == HobKnobObject.transform)
        {
            IsHobOn = false;
            // Start of Joshua Cossar's Added Code
            Registry.CoreGameInfrastructureObject.SFXSource.Stop();
            // End of Joshua Cossar's Added Code
        }
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

                    Registry.CoreGameInfrastructureObject.SFXSource.PlayOneShot(Registry.CoreGameInfrastructureObject.EggTimerAlarm); // Added by Joshua Cossar
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

            // Determine if the player has interacted with the hob dial to turn it off.
            if (Input.touchCount > 0) // if touch input is used
            {
                UnityEngine.Touch touch = Input.GetTouch(0); // get the first finger (only guaranteed input)

                HandleTouch(touch.position); // handle position adjustments, using finger position on-screen.
            }
            else if (Input.GetMouseButton(0)) // if mouse click is used
            {
                HandleTouch(Input.mousePosition); // handle position adjustments, using mouse position.
            }

            EggTimerObject.transform.position = EggTimerPosition; // Position the egg timer on-screen.
        }
    }
}
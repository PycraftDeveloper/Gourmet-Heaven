using System.Collections;
using UnityEngine;

public class Buns_MiniGameManager : MonoBehaviour
{
    public GameObject HandPointerObject;
    public GameObject FireIndicatorObject;
    public GameObject EggTimerObject;
    public GameObject HobKnobObject;

    public GameObject MiniGameFailedPopUp;
    public GameObject MiniGameWinPopUp;

    public GameObject BunsMiniGameTutorial;

    private Animator EggTimerAnimator;

    private float CurrentMiniGameDuration = 0;
    private float TargetMiniGameDuration;

    private bool MiniGameLocked = false;

    private bool IsHobOn = true;

    private Vector3 EggTimerPosition = new Vector3(-3.53f, -0.1f, 100);

    private void ReturnToKitchen()
    {
        Registry.GameManagerObject.ChangeScene();
    }

    private void OnMiniGameFailed()
    {
        MiniGameLocked = true;
        MiniGameFailedPopUp.SetActive(true);
        Invoke("ReturnToKitchen", 2);
    }

    private void ShowMiniGameSucsess()
    {
        MiniGameWinPopUp.SetActive(true);
        Registry.PlayerObject.GetComponent<Player>().HoldingMeal = Constants.BAO_BUNS;
        Invoke("ReturnToKitchen", 2);
    }

    private void OnMiniGameWin()
    {
        MiniGameLocked = true;

        Registry.BunsMGTutorialShown = true;

        ShowMiniGameSucsess();
    }

    public void OnContinueButtonClicked()
    {
        BunsMiniGameTutorial.SetActive(false);
        MiniGameLocked = false;
        Registry.NotInTutorialScreenTimeModifier = 1;

        Registry.GameManagerObject.SFXSource.clip = Registry.GameManagerObject.BoilingWater;
        Registry.GameManagerObject.SFXSource.volume = Registry.SFXVolume;
        Registry.GameManagerObject.SFXSource.Play();
        Registry.GameManagerObject.SFXSource.loop = true;
        Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.EggtimerTicking);
    }

    private void Start()
    {
        TargetMiniGameDuration = Random.Range(Constants.BUNS_TIME_DELAY[0], Constants.BUNS_TIME_DELAY[1]);

        EggTimerAnimator = EggTimerObject.GetComponent<Animator>();

        if (!Registry.BunsMGTutorialShown)
        {
            BunsMiniGameTutorial.SetActive(true);
            MiniGameLocked = true;
            Registry.NotInTutorialScreenTimeModifier = 0;
        }
        if (!MiniGameLocked)
        {
            // Start of Joshua Cossar's Added Code
            Registry.GameManagerObject.SFXSource.clip = Registry.GameManagerObject.BoilingWater;
            Registry.GameManagerObject.SFXSource.volume = Registry.SFXVolume;
            Registry.GameManagerObject.SFXSource.Play();
            Registry.GameManagerObject.SFXSource.loop = true;
            Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.EggtimerTicking);
            // End of Joshua Cossar's Added Code
        }
    }

    private void HandleTouch(Vector2 TouchPosition)
    {
        Vector2 WorldPosition = Camera.main.ScreenToWorldPoint(TouchPosition);
        Collider2D RaycastHit = Physics2D.OverlapPoint(WorldPosition);

        if (RaycastHit != null && RaycastHit.transform == HobKnobObject.transform)
        {
            IsHobOn = false;
            Registry.GameManagerObject.SFXSource.Stop();
        }
    }

    private IEnumerator RotateHobKnob(GameObject HobKnob)
    {
        float Duration = 0.5f;
        float RunTime = 0;

        Transform HobKnobTransform = HobKnob.transform;

        while (RunTime < Duration)
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
            CurrentMiniGameDuration += Time.deltaTime;

            if (CurrentMiniGameDuration > TargetMiniGameDuration)
            {
                if (!HandPointerObject.activeSelf)
                {
                    HandPointerObject.SetActive(true);

                    EggTimerPosition.y += Constants.ACTIVE_EGG_TIMER_DISPLACEMENT;

                    Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.EggtimerAlarm); // Added by Joshua Cossar
                    EggTimerAnimator.SetBool("Alarm", true);
                }

                if (CurrentMiniGameDuration > TargetMiniGameDuration + Constants.BUNS_REACTION_THRESHOLD)
                {
                    OnMiniGameFailed();
                }

                if (!IsHobOn)
                {
                    HandPointerObject.SetActive(false);
                    FireIndicatorObject.SetActive(false);
                    MiniGameLocked = true;
                    StartCoroutine(RotateHobKnob(HobKnobObject));
                    EggTimerAnimator.enabled = false;
                    EggTimerPosition.y -= Constants.ACTIVE_EGG_TIMER_DISPLACEMENT;
                    Invoke("OnMiniGameWin", 2);
                }
            }
            else if (!IsHobOn)
            {
                OnMiniGameFailed();
            }

            if (Input.touchCount > 0) // if touch input is used
            {
                UnityEngine.Touch touch = Input.GetTouch(0); // get the first finger (only guaranteed input)

                HandleTouch(touch.position); // handle position adjustments, using finger position on-screen.
            }
            else if (Input.GetMouseButton(0)) // if mouse click is used
            {
                HandleTouch(Input.mousePosition); // handle position adjustments, using mouse position.
            }

            EggTimerObject.transform.position = EggTimerPosition;
        }
    }
}
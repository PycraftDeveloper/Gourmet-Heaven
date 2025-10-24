using UnityEngine;

public class MainMenuManagerScript : MonoBehaviour
{
    public GameObject IntroAnimObject;
    public Camera BackgroundCamera; // Used to change the letterbox colour.

    private IntroSequenceManager IntroAnim;
    private Coroutine IntroAnimCoroutine; // Allows the coroutine to run in the background.

    public void Start()
    {
        IntroAnim = IntroAnimObject.GetComponent<IntroSequenceManager>();
        Application.targetFrameRate = Mathf.Max(60, (int)Screen.currentResolution.refreshRateRatio.value); // Set the target frame rate to be the maximum between 60 or the target device refresh rate.
    }

    public void OnPlayButtonClick()
    {
        Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.ButtonClickSound);
        Registry.GameManagerObject.ChangeScene(Constants.LEVEL_SELECTION_MENU);
    }

    public void OnShopButtonClick()
    {
        Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.ButtonClickSound);
        Registry.GameManagerObject.ChangeScene(Constants.SHOP_MENU);
    }

    public void OnOptionsButtonClick()
    {
        Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.ButtonClickSound);
        Registry.GameManagerObject.ChangeScene(Constants.OPTIONS_MENU);
    }

    public void OnCreditsButtonClick()
    {
        Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.ButtonClickSound);
        Registry.GameManagerObject.ChangeScene(Constants.CREDITS_MENU);
    }

    public void OnQuitButtonClick()
    {
        Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.ButtonClickSound);
        Registry.GameManagerObject.QuitGame();
    }

    private void Update()
    {
        if (!Registry.IntroSequencePlayed) // If the intro-sequence has not been played, then play it and don't play it again.
        {
            IntroAnimCoroutine = Registry.GameManagerObject.StartCoroutine(IntroAnim.Play());
            Registry.IntroSequencePlayed = true;
        }

        if (IntroAnimCoroutine != null) // if the intro sequence is playing.
        {
            IntroAnimObject.SetActive(true); // Show the intro-sequence.
            // Detect input to skip the intro-sequence.
            if (Input.touchCount > 0) // if touch input is used
            {
                Registry.GameManagerObject.StopCoroutine(IntroAnimCoroutine);
                IntroAnimCoroutine = null;
            }
            else if (Input.GetMouseButton(0)) // if mouse click is used
            {
                Registry.GameManagerObject.StopCoroutine(IntroAnimCoroutine);
                IntroAnimCoroutine = null;
            }

            if (IntroAnim.AnimationSequencePlaying == false)
            {
                IntroAnimCoroutine = null;
            }
        }
        else // If the intro-sequence is not playing then hide it and reset the letterbox colour.
        {
            IntroAnimObject.SetActive(false);
            BackgroundCamera.backgroundColor = new Color(0, 0, 0, 1);
        }
    }
}
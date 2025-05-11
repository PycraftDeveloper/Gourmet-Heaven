using System.Collections;
using UnityEngine;

public class MainMenuManagerScript : MonoBehaviour
{
    public GameObject IntroAnimObject;
    private IntroSequenceManager IntroAnim;

    private Coroutine IntroAnimCoroutine;

    public void Start()
    {
        IntroAnim = IntroAnimObject.GetComponent<IntroSequenceManager>();
        Application.targetFrameRate = Mathf.Max(60, (int)Screen.currentResolution.refreshRateRatio.value);
    }

    public void OnPlayButtonClick()
    {
        Registry.GameManagerObject.ChangeScene(Constants.LEVEL_SELECTION_MENU);
    }

    public void OnShopButtonClick()
    {
        Registry.GameManagerObject.ChangeScene(Constants.SHOP_MENU);
    }

    public void OnOptionsButtonClick()
    {
        Registry.GameManagerObject.ChangeScene(Constants.OPTIONS_MENU);
    }

    public void OnCreditsButtonClick()
    {
        Registry.GameManagerObject.ChangeScene(Constants.CREDITS_MENU);
    }

    public void OnQuitButtonClick()
    {
        Registry.GameManagerObject.QuitGame();
    }

    private void Update()
    {
        if (!Registry.IntroSequencePlayed)
        {
            IntroAnimCoroutine = Registry.GameManagerObject.StartCoroutine(IntroAnim.Play());
            Registry.IntroSequencePlayed = true;
        }

        if (IntroAnim.IsFinished)
        {
            IntroAnimObject.SetActive(false);
        }

        if (Input.touchCount > 0) // if touch input is used
        {
            Registry.GameManagerObject.StopCoroutine(IntroAnimCoroutine);
            IntroAnimObject.SetActive(false);
        }
        else if (Input.GetMouseButton(0)) // if mouse click is used
        {
            Registry.GameManagerObject.StopCoroutine(IntroAnimCoroutine);
            IntroAnimObject.SetActive(false);
        }
    }
}
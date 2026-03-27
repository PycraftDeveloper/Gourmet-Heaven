using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenuManagerScript : MonoBehaviour
{
    [Header("UI")]
    public Canvas MenuCanvas;

    [Header("Background")]
    public RawImage BackgroundImage;

    [Header("Settings UI")]
    public Button PlayerControlsSwitchButton;

    public Slider SFXSlider;
    public Slider MusicSlider;
    private TextMeshProUGUI PlayerControlsSwitchButtonText;

    [Header("Navigable Menus")]
    public GameObject MainMenuPrefab;

    public void Start() // Set the starting values for the settings to what the game currently has set.
    {
        MenuCanvas.worldCamera = Camera.main;
        MenuCanvas.sortingLayerName = "UI";

        PlayerControlsSwitchButtonText = PlayerControlsSwitchButton.GetComponentInChildren<TextMeshProUGUI>();
        SFXSlider.value = Registry.SFXVolume;
        MusicSlider.value = Registry.MusicVolume;

        // use "Registry.GameManagerObject.FrameTexture;" for no blur
        // use "Registry.GameManagerObject.FrameTexture;" for blur
        // use transparency if you want the blur to appear to 'fade in'
        BackgroundImage.texture = Registry.CGI.BlurredFrameTexture;
    }

    public void OnSwitchControllerPositionButtonClicked() // Change the joystick position, the code will later determine what this change means.
    {
        Registry.CGI.SFXSource.PlayOneShot(Registry.CGI.ButtonClickSound);
        if (Registry.JoystickScreenPosition == Constants.LEFT)
        {
            Registry.JoystickScreenPosition = Constants.RIGHT;
        }
        else
        {
            Registry.JoystickScreenPosition = Constants.LEFT;
        }
    }

    public void OnBackButtonClicked()
    {
        Registry.CGI.SFXSource.PlayOneShot(Registry.CGI.ButtonClickSound);
        Instantiate(MainMenuPrefab);
        Destroy(gameObject);
    }

    public void Update()
    {
        PlayerControlsSwitchButtonText.text = Registry.JoystickScreenPosition;
        Registry.SFXVolume = SFXSlider.value;
        Registry.MusicVolume = MusicSlider.value;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Instantiate(MainMenuPrefab);
            Destroy(gameObject);
        }
    }
}
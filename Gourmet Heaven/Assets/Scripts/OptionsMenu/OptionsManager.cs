using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenuManagerScript : MonoBehaviour
{
    public Button PlayerControlsSwitchButton;
    public Slider SFXSlider;
    public Slider MusicSlider;
    public Canvas MenuCanvas;

    private TextMeshProUGUI PlayerControlsSwitchButtonText;

    public void Start() // Set the starting values for the settings to what the game currently has set.
    {
        MenuCanvas.worldCamera = Camera.main;
        MenuCanvas.sortingLayerName = "UI";

        PlayerControlsSwitchButtonText = PlayerControlsSwitchButton.GetComponentInChildren<TextMeshProUGUI>();
        SFXSlider.value = Registry.SFXVolume;
        MusicSlider.value = Registry.MusicVolume;
    }

    public void OnSwitchControllerPositionButtonClicked() // Change the joystick position, the code will later determine what this change means.
    {
        Registry.CoreGameInfrastructureObject.SFXSource.PlayOneShot(Registry.CoreGameInfrastructureObject.ButtonClickSound);
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
        Registry.CoreGameInfrastructureObject.SFXSource.PlayOneShot(Registry.CoreGameInfrastructureObject.ButtonClickSound);
        Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.PREVIOUS_MENU);
    }

    public void Update() // Get the values for the slider and apply them to the game settings.
    {
        PlayerControlsSwitchButtonText.text = Registry.JoystickScreenPosition;
        Registry.SFXVolume = SFXSlider.value;
        Registry.MusicVolume = MusicSlider.value;

        if (Input.GetKeyDown(KeyCode.Escape)) // Handle the user's keyboard input for Windows builds.
        {
            Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.PREVIOUS_MENU);
        }
    }
}
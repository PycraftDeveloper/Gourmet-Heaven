using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class OptionsMenuManagerScript : MonoBehaviour
{
    public Button PlayerControlsSwitchButton;

    private TextMeshProUGUI PlayerControlsSwitchButtonText;

    public void Start()
    {
        PlayerControlsSwitchButtonText = PlayerControlsSwitchButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnSwitchControllerPositionButtonClicked()
    {
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
        SceneManager.LoadScene("MainMenu");
    }

    public void Update()
    {
        PlayerControlsSwitchButtonText.text = Registry.JoystickScreenPosition;
    }
}
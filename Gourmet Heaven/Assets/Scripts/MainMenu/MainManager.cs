using UnityEngine;

public class MainMenuManagerScript : MonoBehaviour
{
    public Canvas MenuCanvas;

    public AudioClip BackgroundMusic;

    public void Start()
    {
        Destroy(Registry.CoreGameInfrastructureObject.BlurredFrameTexture);

        Registry.CoreGameInfrastructureObject.GameMusicSource.Stop();

        if (Registry.CoreGameInfrastructureObject.musicSource.clip != BackgroundMusic)
        {
            Registry.CoreGameInfrastructureObject.musicSource.clip = BackgroundMusic;
            Registry.CoreGameInfrastructureObject.musicSource.loop = true;
        }
        Registry.CoreGameInfrastructureObject.musicSource.Play();

        MenuCanvas.worldCamera = Camera.main;
        Registry.PlayerScore = 0;
    }

    public void OnPlayButtonClick()
    {
        Registry.CoreGameInfrastructureObject.SFXSource.PlayOneShot(Registry.CoreGameInfrastructureObject.ButtonClickSound);
        Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.LEVEL_SELECTION_MENU);
    }

    public void OnShopButtonClick()
    {
        Registry.CoreGameInfrastructureObject.SFXSource.PlayOneShot(Registry.CoreGameInfrastructureObject.ButtonClickSound);
        Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.SHOP_MENU);
    }

    public void OnSettingsButtonClick()
    {
        Registry.CoreGameInfrastructureObject.SFXSource.PlayOneShot(Registry.CoreGameInfrastructureObject.ButtonClickSound);
        Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.SETTINGS_MENU);
    }

    public void OnCreditsButtonClick()
    {
        Registry.CoreGameInfrastructureObject.SFXSource.PlayOneShot(Registry.CoreGameInfrastructureObject.ButtonClickSound);
        Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.CREDITS_MENU);
    }

    public void OnQuitButtonClick()
    {
        Registry.CoreGameInfrastructureObject.SFXSource.PlayOneShot(Registry.CoreGameInfrastructureObject.ButtonClickSound);
        Registry.CoreGameInfrastructureObject.QuitGame();
    }
}
using UnityEngine;

public class MainMenuManagerScript : MonoBehaviour
{
    [Header("UI")]
    public Canvas MenuCanvas;

    [Header("Navigable Menus")]
    public GameObject LevelSelectionMenuPrefab;

    public GameObject ShopMenuPrefab;
    public GameObject SettingsMenuPrefab;
    public GameObject CreditsMenuPrefab;

    [Header("Music")]
    public AudioClip BackgroundMusic;

    public void Start()
    {
        Destroy(Registry.CGI.BlurredFrameTexture);

        Registry.CGI.GameMusicSource.Stop();

        if (Registry.CGI.musicSource.clip != BackgroundMusic)
        {
            Registry.CGI.musicSource.clip = BackgroundMusic;
            Registry.CGI.musicSource.loop = true;
        }
        Registry.CGI.musicSource.Play();

        MenuCanvas.worldCamera = Camera.main;
        Registry.PlayerScore = 0;
    }

    public void OnPlayButtonClick()
    {
        Registry.CGI.SFXSource.PlayOneShot(Registry.CGI.ButtonClickSound);
        Instantiate(LevelSelectionMenuPrefab);
        Destroy(gameObject);
    }

    public void OnShopButtonClick()
    {
        Registry.CGI.SFXSource.PlayOneShot(Registry.CGI.ButtonClickSound);
        Instantiate(ShopMenuPrefab);
        Destroy(gameObject);
    }

    public void OnSettingsButtonClick()
    {
        Registry.CGI.SFXSource.PlayOneShot(Registry.CGI.ButtonClickSound);
        Instantiate(SettingsMenuPrefab);
        Destroy(gameObject);
    }

    public void OnCreditsButtonClick()
    {
        Registry.CGI.SFXSource.PlayOneShot(Registry.CGI.ButtonClickSound);
        Instantiate(CreditsMenuPrefab);
        Destroy(gameObject);
    }

    public void OnQuitButtonClick()
    {
        Registry.CGI.SFXSource.PlayOneShot(Registry.CGI.ButtonClickSound);
        Registry.CGI.QuitGame();
    }
}
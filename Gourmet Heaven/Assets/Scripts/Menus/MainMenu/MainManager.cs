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
        Registry.InGame = false;
    }

    public void OnPlayButtonClick()
    {
        Registry.CGI.PlayClickSound();
        Instantiate(LevelSelectionMenuPrefab);
    }

    public void OnShopButtonClick()
    {
        Registry.CGI.PlayClickSound();
        Instantiate(ShopMenuPrefab);
    }

    public void OnSettingsButtonClick()
    {
        Registry.CGI.PlayClickSound();
        Registry.CGI.RenderGameSceneToFrameBuffer();
        Instantiate(SettingsMenuPrefab);
    }

    public void OnCreditsButtonClick()
    {
        Registry.CGI.PlayClickSound();
        Instantiate(CreditsMenuPrefab);
    }

    public void OnQuitButtonClick()
    {
        Registry.CGI.QuitGame();
    }
}
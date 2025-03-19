using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    [Header("AudioSources")]
    [SerializeField] private AudioSource musicSource;

    [SerializeField] private AudioSource SFXSource;

    [Header("AudioClips")]
    public AudioClip bgm_InGame;

    public AudioClip bgm_MainMenu;
    public AudioClip bgm_MiniGame;

    public AudioClip footsteps;
    public AudioClip audioClip2;
    public AudioClip audioClip3;
    public AudioClip audioClip4;

    private Stack<string> MenuStack = new Stack<string>();

    public Texture2D FrameTexture;
    public Texture2D BlurredFrameTexture;

    public Material BlurredMaterial;

    private void Awake()
    {
        if (Registry.GameManagerObject == null)
        {
            DontDestroyOnLoad(gameObject);
            Registry.GameManagerObject = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        MenuStack.Push(Constants.MAIN_MENU);
        musicSource.clip = bgm_MainMenu;
        musicSource.volume = Registry.MusicVolume;
        musicSource.Play();
    }

    private string HandleSceneStack(string sceneName)
    {
        if (sceneName == "")
        {
            MenuStack.Pop();
            sceneName = MenuStack.Pop();
        }

        MenuStack.Push(sceneName);

        return sceneName;
    }

    private void EnableLevelObjects(string sceneName)
    {
        if (Registry.JoystickObject != null)
        {
            Registry.JoystickObject.gameObject.SetActive(true);
        }
        if (Registry.PlayerObject != null)
        {
            Registry.PlayerObject.gameObject.SetActive(true);
        }
        foreach (GameObject Customer in Registry.Customers)
        {
            if (Customer.GetComponent<Customer>().CurrentLocation == sceneName)
            {
                Customer.SetActive(true);
            }
        }
    }

    private void DisableLevelObjects()
    {
        if (Registry.JoystickObject != null)
        {
            Registry.JoystickObject.gameObject.SetActive(false);
        }
        if (Registry.PlayerObject != null)
        {
            Registry.PlayerObject.gameObject.SetActive(false);
        }
        foreach (GameObject Customer in Registry.Customers)
        {
            Customer.SetActive(false);
        }
    }

    public void ChangeScene(string sceneName = "")
    {
        sceneName = HandleSceneStack(sceneName);

        if (sceneName != Constants.KITCHEN || sceneName != Constants.RESTAURANT)
        {
            if (Registry.InGameLevel)
            {
                Camera camera = Camera.main;

                int width = (int)(Camera.main.rect.width * Screen.width);
                int height = (int)(Camera.main.rect.height * Screen.height);
                float x_offset = (Screen.width - width) / 2;
                float y_offset = (Screen.height - height) / 2;

                FrameTexture = new Texture2D(width, height);
                BlurredFrameTexture = new Texture2D(width, height);

                RenderTexture SceneContents = new RenderTexture(Screen.width, Screen.height, 24);
                RenderTexture BlurredSceneContents = new RenderTexture(Screen.width, Screen.height, 24);

                camera.targetTexture = SceneContents;
                camera.Render();

                Graphics.SetRenderTarget(SceneContents);
                FrameTexture.ReadPixels(new Rect(x_offset, y_offset, Screen.width, Screen.height), 0, 0);
                FrameTexture.Apply();

                Graphics.Blit(SceneContents, BlurredSceneContents, BlurredMaterial);
                Graphics.SetRenderTarget(BlurredSceneContents);
                BlurredFrameTexture.ReadPixels(new Rect(x_offset, y_offset, Screen.width, Screen.height), 0, 0);
                BlurredFrameTexture.Apply();

                camera.targetTexture = null;
                Graphics.SetRenderTarget(null);
            }
            Registry.InGameLevel = false;
            DisableLevelObjects();
        }

        if (sceneName == Constants.BUNS_MG)
        {
        }
        else if (sceneName == Constants.CREDITS_MENU)
        {
        }
        else if (sceneName == Constants.KITCHEN)
        {
        }
        else if (sceneName == Constants.LEVEL_SELECTION_MENU)
        {
        }
        else if (sceneName == Constants.MAIN_MENU)
        {
        }
        else if (sceneName == Constants.OPTIONS_MENU)
        {
        }
        else if (sceneName == Constants.PAUSE_MENU)
        {
        }
        else if (sceneName == Constants.PHO_MG)
        {
        }
        else if (sceneName == Constants.RESTAURANT)
        {
        }
        else if (sceneName == Constants.RICE_MG)
        {
        }
        else if (sceneName == Constants.SHOP_MENU)
        {
        }
        else if (sceneName == Constants.SUSHI_MG)
        {
        }

        Registry.CurrentSceneName = sceneName;

        SceneManager.LoadScene(sceneName);

        if (sceneName == Constants.KITCHEN || sceneName == Constants.RESTAURANT)
        {
            Registry.InGameLevel = true;
            EnableLevelObjects(sceneName);
        }

        if (sceneName == Constants.KITCHEN)
        {
            if (musicSource.clip != bgm_InGame)
            {
                musicSource.clip = bgm_InGame;
                musicSource.volume = Registry.MusicVolume;
                musicSource.Play();
            }
        }
        if (sceneName == Constants.MAIN_MENU)
        {
            if (musicSource.clip != bgm_MainMenu)
            {
                musicSource.clip = bgm_MainMenu;
                musicSource.volume = Registry.MusicVolume;
                musicSource.Play();
            }
        }
        if (sceneName == Constants.BUNS_MG || sceneName == Constants.PHO_MG || sceneName == Constants.RICE_MG || sceneName == Constants.SUSHI_MG)
        {
            musicSource.clip = bgm_MiniGame;
            musicSource.volume = Registry.MusicVolume;
            musicSource.Play();
        }
    }

    private void Update()
    {
        musicSource.volume = Registry.MusicVolume;
    }
}
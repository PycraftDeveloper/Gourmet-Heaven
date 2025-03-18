using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
                FrameTexture = new Texture2D(Screen.width, Screen.height);
                BlurredFrameTexture = new Texture2D(Screen.width, Screen.height);

                RenderTexture SceneContents = new RenderTexture(Screen.width, Screen.height, 24);
                RenderTexture BlurredSceneContents = new RenderTexture(Screen.width, Screen.height, 24);

                Camera camera = Camera.main;
                camera.targetTexture = SceneContents;
                camera.Render();

                Graphics.SetRenderTarget(SceneContents);
                FrameTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
                FrameTexture.Apply();

                Graphics.Blit(SceneContents, BlurredSceneContents, BlurredMaterial);
                Graphics.SetRenderTarget(BlurredSceneContents);
                BlurredFrameTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
                BlurredFrameTexture.Apply();
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
    }

    private void Update()
    {
        musicSource.volume = Registry.MusicVolume;
    }
}
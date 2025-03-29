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

    private SavedDataManager savedDataManager;

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
        savedDataManager = new SavedDataManager();
        savedDataManager.Load();

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
        foreach (GameObject CustomerGameObject in Registry.Customers)
        {
            Customer _Customer = CustomerGameObject.GetComponent<Customer>();
            if (_Customer.CurrentLocation == sceneName)
            {
                CustomerGameObject.SetActive(true);
                _Customer.ReAssociateAnimations();
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
        foreach (GameObject CustomerGameObject in Registry.Customers)
        {
            CustomerGameObject.SetActive(false);
        }
    }

    public void AfterSceneChange()
    {
        string sceneName = Registry.CurrentSceneName;

        if (sceneName != Constants.KITCHEN && sceneName != Constants.RESTAURANT)
        {
            if (Registry.InGameLevel)
            {
                Registry.InGameLevel = false;
                DisableLevelObjects();
            }
        }

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

        if (Registry.PlayerObject != null)
        {
            Registry.PlayerObject.SceneChanged = true;
        }

        if (Registry.JoystickObject != null)
        {
            Registry.JoystickObject.OnSceneChanged();
        }

        for (int i = Registry.Customers.Count - 1; i >= 0; i--)
        {
            Customer thisCustomer = Registry.Customers[i].GetComponent<Customer>();

            if (thisCustomer.DeSpawn)
            {
                Registry.LevelManagerObject.CustomerTableArrangement[thisCustomer.CustomerTablePosition] = null;
                Registry.Customers.Remove(thisCustomer.gameObject);
                Destroy(thisCustomer.gameObject);
            }
            else if (Registry.CurrentSceneName != thisCustomer.CurrentLocation)
            {
                Registry.Customers[i].SetActive(false);
            }
        }
    }

    public void QuitGame()
    {
        savedDataManager.Save();
        Application.Quit();
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

                GameObject[] AllGameObjects = FindObjectsByType<GameObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
                List<GameObject> UI_GameObjects = new List<GameObject>();
                foreach (GameObject gameObject in AllGameObjects)
                {
                    if (gameObject.layer == LayerMask.NameToLayer("UI"))
                    {
                        UI_GameObjects.Add(gameObject);
                        gameObject.SetActive(false);
                    }
                }

                camera.targetTexture = SceneContents;
                camera.Render();

                Graphics.SetRenderTarget(SceneContents);
                FrameTexture.ReadPixels(new Rect(x_offset, y_offset, Screen.width, Screen.height), 0, 0);
                FrameTexture.Apply();

                Graphics.Blit(SceneContents, BlurredSceneContents, BlurredMaterial);
                Graphics.SetRenderTarget(BlurredSceneContents);
                BlurredFrameTexture.ReadPixels(new Rect(x_offset, y_offset, Screen.width, Screen.height), 0, 0);
                BlurredFrameTexture.Apply();

                foreach (GameObject gameObject in UI_GameObjects)
                {
                    gameObject.SetActive(true);
                }

                camera.targetTexture = null;
                Graphics.SetRenderTarget(null);
            }
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
            foreach (GameObject _Customer in Registry.Customers)
            {
                Destroy(_Customer);
            }
            Registry.Customers.Clear();

            if (Registry.PlayerObject != null)
            {
                Destroy(Registry.PlayerObject);
                Registry.PlayerObject = null;
            }

            if (Registry.LevelManagerObject != null)
            {
                Destroy(Registry.LevelManagerObject);
                Registry.LevelManagerObject = null;
            }

            if (Registry.JoystickObject != null)
            {
                Destroy(Registry.JoystickObject);
                Registry.JoystickObject = null;
            }
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
    }

    private void Update()
    {
        musicSource.volume = Registry.MusicVolume;
    }
}
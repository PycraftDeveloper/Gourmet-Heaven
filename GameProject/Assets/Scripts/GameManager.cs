using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // This class handles scene changes and the changes to the game state that results from this process.

    // start - this section of code was worked on by Joshua Cossar (v)
    [Header("AudioSources")]
    [SerializeField] private AudioSource musicSource;

    [SerializeField] public AudioSource SFXSource;

    [Header("AudioClips")]
    public AudioClip bgm_InGame;

    public AudioClip bgm_MainMenu;
    public AudioClip bgm_MiniGame;

    public AudioClip footsteps;
    public AudioClip CuttingSound;
    public AudioClip MangoFinish;
    public AudioClip CustomerFinish1;
    public AudioClip CustomerFinish2;
    public AudioClip CustomerFinish3;
    // end - this section of code was worked on by Joshua Cossar (^)

    private Stack<string> MenuStack = new Stack<string>(); // stores a stack containing all the menus previously visited

    // start - blurred background setup
    public Texture2D FrameTexture;

    public Texture2D BlurredFrameTexture;

    public Material BlurredMaterial;
    // end - blurred background setup

    private SavedDataManager savedDataManager; // used to save/load the game state.

    private void Awake()
    {
        // Used to ensure that only one instance of the game manager can ever exist, and allows other scripts
        // to reference it through the registry.
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
        savedDataManager.Load(); // setup the data manager, and load any previous saved data.

        MenuStack.Push(Constants.MAIN_MENU); // Pre-fill the menu stack with the initial game setup.

        // start - this section of code was worked on by Joshua Cossar (v)
        musicSource.clip = bgm_MainMenu;
        musicSource.volume = Registry.MusicVolume;
        musicSource.Play();
        // end - this section of code was worked on by Joshua Cossar (^)
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

    public void ResetGameLevel()
    {
        Registry.PlayerScore = 0;
        Registry.MaxScore = 0;

        foreach (GameObject _Customer in Registry.Customers)
        {
            Destroy(_Customer);
        }
        Registry.Customers.Clear();

        if (Registry.PlayerObject != null)
        {
            Destroy(Registry.PlayerObject.gameObject);
            Registry.PlayerObject = null;
        }

        if (Registry.LevelManagerObject != null)
        {
            Destroy(Registry.LevelManagerObject.gameObject);
            Registry.LevelManagerObject = null;
        }

        if (Registry.JoystickObject != null)
        {
            Destroy(Registry.JoystickObject.gameObject);
            Registry.JoystickObject = null;
        }

        if (Registry.UIManagerObject != null)
        {
            Destroy(Registry.UIManagerObject.gameObject);
            Registry.UIManagerObject = null;
        }

        Registry.LevelRunTime = 0;
        Registry.PlayerScore = 0;
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
            CustomerCore _Customer = CustomerGameObject.GetComponent<CustomerCore>();
            if (_Customer.CurrentLocation == sceneName)
            {
                CustomerGameObject.SetActive(true);
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

        if (sceneName == Constants.END_MENU)
        {
            ResetGameLevel();
        }

        if (Registry.PlayerObject != null)
        {
            Registry.PlayerObject.SceneChanged = true;
        }

        if (Registry.JoystickObject != null)
        {
            Registry.JoystickObject.OnSceneChanged();
        }

        if (Registry.UIManagerObject != null)
        {
            Registry.UIManagerObject.OnSceneChanged();
        }

        for (int i = Registry.Customers.Count - 1; i >= 0; i--)
        {
            CustomerCore thisCustomer = Registry.Customers[i].GetComponent<CustomerCore>();

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

    private void OnApplicationFocus(bool hasFocus)
    {
        Registry.GameInBackground = !hasFocus;
    }

    public void RenderGameSceneToFrameBuffer()
    {
        if (Registry.GameInBackground)
        {
            return;
        }

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

    public void ChangeScene(string sceneName = "")
    {
        sceneName = HandleSceneStack(sceneName);

        if (sceneName != Constants.KITCHEN || sceneName != Constants.RESTAURANT)
        {
            if (Registry.UIManagerObject != null)
            {
                Registry.UIManagerObject.gameObject.SetActive(false);
            }

            if (Registry.InGameLevel)
            {
                RenderGameSceneToFrameBuffer();
            }
        }

        if (sceneName == Constants.KITCHEN || sceneName == Constants.RESTAURANT)
        {
            if (Registry.UIManagerObject != null)
            {
                Registry.UIManagerObject.gameObject.SetActive(true);
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
            ResetGameLevel();
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
        else if (sceneName == Constants.END_MENU)
        {
        }

        Registry.CurrentSceneName = sceneName;

        SceneManager.LoadScene(sceneName);
    }

    private void Update()
    {
        musicSource.volume = Registry.MusicVolume;
        SFXSource.volume = Registry.SFXVolume;

        if (Registry.LevelRunTime > 0)
        {
            Registry.LevelRunTime -= Time.deltaTime;
            if (Registry.LevelRunTime <= 0)
            {
                ChangeScene(Constants.END_MENU);
            }
        }
    }
}
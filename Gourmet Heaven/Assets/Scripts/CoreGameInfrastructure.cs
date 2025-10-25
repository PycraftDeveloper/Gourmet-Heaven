using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR

using UnityEditor;

#endif

// Game scene - The Kitchen or Restaurant scenes but no others.

public class CoreGameInfrastructure : MonoBehaviour
{
    // This class handles scene changes and the changes to the game state that results from this process.

    // start - this section of code was worked on by Joshua Cossar (v)
    [Header("AudioSources")]
    [SerializeField] private AudioSource musicSource;

    [SerializeField] private AudioSource GameMusicSource;

    [SerializeField] public AudioSource SFXSource;

    [Header("AudioClips")]
    public AudioClip bgm_InGame;

    public AudioClip bgm_MainMenu;
    public AudioClip bgm_MiniGame;

    public AudioClip CashRegisterNoise;
    public AudioClip CuttingSound;
    public AudioClip MangoFinish;
    public AudioClip BoilingWater;
    public AudioClip EggTimerTicking;
    public AudioClip EggTimerAlarm;
    public AudioClip SoupSplash1;
    public AudioClip SoupSplash2;
    public AudioClip SushiSound;
    public AudioClip CustomerFinish1;
    public AudioClip CustomerFinish2;
    public AudioClip CustomerFinish3;
    public AudioClip RestaurantAmbience;
    public AudioClip ButtonClickSound;
    // end - this section of code was worked on by Joshua Cossar (^)

    private Stack<string> MenuStack = new Stack<string>(); // stores a stack containing all the menus previously visited

    // start - blurred background set-up
    public Texture2D FrameTexture;

    public Texture2D BlurredFrameTexture;

    public Material BlurredMaterial;
    // end - blurred background set-up

    private SavedDataManager savedDataManager;

    public GameObject MainMenuPrefab;
    public GameObject IntroSequAnimPrefab;
    public GameObject CreditsMenuPrefab;
    public GameObject SettingsMenuPrefab;
    public GameObject ShopMenuPrefab;
    public GameObject LevelSelectionMenuPrefab;
    public GameObject GameTutorialMenuPrefab;
    public GameObject PauseMenuPrefab;
    public GameObject EndMenuPrefab;
    public GameObject Sushi_MG_MenuPrefab;
    public GameObject Sushi_MG_TutorialMenuPrefab;
    public GameObject Buns_MG_MenuPrefab;
    public GameObject Buns_MG_TutorialMenuPrefab;
    public GameObject Pho_MG_MenuPrefab;
    public GameObject Pho_MG_TutorialMenuPrefab;
    public GameObject Rice_MG_MenuPrefab;
    public GameObject Rice_MG_TutorialMenuPrefab;

    private GameObject CurrentMenu;

    private bool SceneChanging = false;
    private string NextSceneName = "";
    private string NextMenuName = "";

    private void Awake()
    {
        if (Registry.CoreGameInfrastructureObject == null)
        {
            DontDestroyOnLoad(gameObject);
            Registry.CoreGameInfrastructureObject = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Application.targetFrameRate = Mathf.Max(60, (int)Screen.currentResolution.refreshRateRatio.value);
        Registry.CoreGameInfrastructureObject.ChangeMenu(Constants.MAIN_MENU);
        ChangeMenu(Constants.INTRO_SEQU_ANIM_MENU, false);
    }

    private void Update()
    {
        if (SceneChanging && SceneManager.GetActiveScene().name == NextSceneName)
        {
            SceneChanging = false;
            if (NextMenuName != "")
            {
                ChangeMenu(NextMenuName);
                NextMenuName = "";
            }
        }
    }

    public void CloseMenu()
    {
        if (CurrentMenu != null)
        {
            Destroy(CurrentMenu);
        }
    }

    public GameObject MenuFinder(string NewMenu)
    {
        if (NewMenu == Constants.MAIN_MENU)
        {
            return Instantiate(MainMenuPrefab);
        }
        else if (NewMenu == Constants.INTRO_SEQU_ANIM_MENU)
        {
            return Instantiate(IntroSequAnimPrefab);
        }
        else if (NewMenu == Constants.CREDITS_MENU)
        {
            return Instantiate(CreditsMenuPrefab);
        }
        else if (NewMenu == Constants.SETTINGS_MENU)
        {
            return Instantiate(SettingsMenuPrefab);
        }
        else if (NewMenu == Constants.SHOP_MENU)
        {
            return Instantiate(ShopMenuPrefab);
        }
        else if (NewMenu == Constants.LEVEL_SELECTION_MENU)
        {
            return Instantiate(LevelSelectionMenuPrefab);
        }
        else if (NewMenu == Constants.GAME_TUTORIAL_MENU)
        {
            return Instantiate(GameTutorialMenuPrefab);
        }
        else if (NewMenu == Constants.PAUSE_MENU)
        {
            return Instantiate(PauseMenuPrefab);
        }
        else if (NewMenu == Constants.END_MENU)
        {
            return Instantiate(EndMenuPrefab);
        }
        else if (NewMenu == Constants.SUSHI_MG_MENU)
        {
            return Instantiate(Sushi_MG_MenuPrefab);
        }
        else if (NewMenu == Constants.SUSHI_MG_TUTORIAL_MENU)
        {
            return Instantiate(Sushi_MG_TutorialMenuPrefab);
        }
        else if (NewMenu == Constants.BUNS_MG_MENU)
        {
            return Instantiate(Buns_MG_MenuPrefab);
        }
        else if (NewMenu == Constants.BUNS_MG_TUTORIAL_MENU)
        {
            return Instantiate(Buns_MG_TutorialMenuPrefab);
        }
        else if (NewMenu == Constants.PHO_MG_MENU)
        {
            return Instantiate(Pho_MG_MenuPrefab);
        }
        else if (NewMenu == Constants.PHO_MG_TUTORIAL_MENU)
        {
            return Instantiate(Pho_MG_TutorialMenuPrefab);
        }
        else if (NewMenu == Constants.RICE_MG_MENU)
        {
            return Instantiate(Rice_MG_MenuPrefab);
        }
        else if (NewMenu == Constants.RICE_MG_TUTORIAL_MENU)
        {
            return Instantiate(Rice_MG_TutorialMenuPrefab);
        }
        else
        {
            Debug.LogError("Menu not found: " + NewMenu);
            return null;
        }
    }

    public void ChangeMenu(string NewMenu, bool ClosePrevious = true)
    {
        if (SceneChanging)
        {
            NextMenuName = NewMenu;
            return;
        }

        if (NewMenu == Constants.PREVIOUS_MENU)
        {
            MenuStack.Pop(); // Remove current menu from stack
            NewMenu = MenuStack.Pop();
        }

        if (ClosePrevious && CurrentMenu != null)
        {
            Destroy(CurrentMenu);
        }

        GameObject NewMenuInstance = MenuFinder(NewMenu);

        if (ClosePrevious)
        {
            MenuStack.Push(NewMenu);
            CurrentMenu = NewMenuInstance;
        }
    }

    public void ChangeScene(string NewScene)
    {
        SceneChanging = true;
        NextSceneName = NewScene;
        SceneManager.LoadScene(NewScene);
        CloseMenu();
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
        }
#endif
    }

    public void RenderGameSceneToFrameBuffer() // Used in the pause menu to create the blurred background.
    {
        if (Registry.GameInBackground || Application.platform == RuntimePlatform.IPhonePlayer) // If the blurred background is not supported or game already in background...
        { // ... When the game is already in the background, don't try to render the background scene as most platforms don't allow this and it causes graphical issues.
            return;
        }

        // Get the camera, and its viewport
        Camera camera = Camera.main;

        int width = (int)(Camera.main.rect.width * Screen.width);
        int height = (int)(Camera.main.rect.height * Screen.height);
        float x_offset = (Screen.width - width) / 2;
        float y_offset = (Screen.height - height) / 2;

        // Create textures the size of the camera's viewport
        FrameTexture = new Texture2D(width, height);
        BlurredFrameTexture = new Texture2D(width, height);

        // Create new render targets, the size of the camera's viewport
        RenderTexture SceneContents = new RenderTexture(Screen.width, Screen.height, 24);
        RenderTexture BlurredSceneContents = new RenderTexture(Screen.width, Screen.height, 24);

        // Get all the GameObjects currently in the scene, so the UI can be hidden.
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

        camera.targetTexture = SceneContents; // Set the camera to render to a render texture
        camera.Render(); // Render the game scene to the render texture.

        Graphics.SetRenderTarget(SceneContents); // Tell the graphics module which texture to use
        FrameTexture.ReadPixels(new Rect(x_offset, y_offset, Screen.width, Screen.height), 0, 0); // Read the frame buffer's content into a texture
        FrameTexture.Apply(); // Apply the changes to the texture

        Graphics.Blit(SceneContents, BlurredSceneContents, BlurredMaterial); // Then render the new texture to the blurred frame buffer, with the blurred material
        Graphics.SetRenderTarget(BlurredSceneContents);
        BlurredFrameTexture.ReadPixels(new Rect(x_offset, y_offset, Screen.width, Screen.height), 0, 0);
        BlurredFrameTexture.Apply();

        foreach (GameObject gameObject in UI_GameObjects)
        {
            gameObject.SetActive(true); // Re-enable the UI so it can be properly set-up for the pause menu.
        }

        camera.targetTexture = null; // reset the camera's render target
        Graphics.SetRenderTarget(null); // reset graphics' render target
    }
}
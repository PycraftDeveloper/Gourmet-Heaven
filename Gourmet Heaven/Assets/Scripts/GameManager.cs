using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

// Game scene - The Kitchen or Restaurant scenes but no others.

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
    // end - this section of code was worked on by Joshua Cossar (^)

    private Stack<string> MenuStack = new Stack<string>(); // stores a stack containing all the menus previously visited

    // start - blurred background set-up
    public Texture2D FrameTexture;

    public Texture2D BlurredFrameTexture;

    public Material BlurredMaterial;
    // end - blurred background set-up

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
        savedDataManager.Load(); // set-up the data manager, and load any previous saved data.

        MenuStack.Push(Constants.MAIN_MENU); // Pre-fill the menu stack with the initial game set-up.

        // start - this section of code was worked on by Joshua Cossar (v)
        musicSource.clip = bgm_MainMenu;
        musicSource.volume = Registry.MusicVolume;
        musicSource.Play();
        // end - this section of code was worked on by Joshua Cossar (^)
    }

    private string HandleSceneStack(string sceneName) // This is used to navigate the stack to find the previously visited scene, when no scene is defined.
    {
        if (sceneName == "")
        {
            MenuStack.Pop(); // Remove current scene from the stack
            sceneName = MenuStack.Pop(); // Remove the previous scene from the stack and get the scene name.
        }

        MenuStack.Push(sceneName); // Add current scene to the stack.

        return sceneName;
    }

    public void ResetGameLevel() // Used to reset both game levels when the player reaches the main menu or end screen.
    {
        Registry.PlayerScore = 0;

        foreach (ForegroundCustomer Customer in Registry.ForegroundCustomers)
        {
            Destroy(Customer.gameObject); // Destroy (and garbage collect) all foreground and background customers in the scene.
        }
        Registry.ForegroundCustomers.Clear(); // Free up space by removing items from array.

        foreach (BackgroundCustomer Customer in Registry.BackgroundCustomers)
        {
            Destroy(Customer.gameObject); // Destroy (and garbage collect) all foreground and background customers in the scene.
        }
        Registry.BackgroundCustomers.Clear(); // Free up space by removing items from array.

        if (Registry.PlayerObject != null) // Check and destroy (garbage collect) the player.
        {
            Destroy(Registry.PlayerObject.gameObject);
            Registry.PlayerObject = null;
        }

        if (Registry.LevelManagerObject != null) // Check and destroy (garbage collect) the level manager.
        {
            Destroy(Registry.LevelManagerObject.gameObject);
            Registry.LevelManagerObject = null;
        }

        if (Registry.JoystickObject != null) // Check and destroy (garbage collect) the joystick.
        {
            Destroy(Registry.JoystickObject.gameObject);
            Registry.JoystickObject = null;
        }

        if (Registry.UIManagerObject != null) // Check and destroy (garbage collect) the UI manager.
        {
            Destroy(Registry.UIManagerObject.gameObject);
            Registry.UIManagerObject = null;
        }

        Registry.LevelRunTime = 0; // Reset level run time
        Registry.PlayerScore = 0; // Reset player score
    }

    private void DisableGameLevelContents()
    {
        foreach (ForegroundCustomer Customer in Registry.ForegroundCustomers)
        {
            Customer.gameObject.SetActive(false); // Enable all foreground customers in the scene.
        }

        foreach (BackgroundCustomer Customer in Registry.BackgroundCustomers)
        {
            Customer.gameObject.SetActive(false); // Enable all background customers in the scene.
        }

        if (Registry.PlayerObject != null)
        {
            Registry.PlayerObject.gameObject.SetActive(false); // Enable the player object in the scene.
        }

        if (Registry.LevelManagerObject != null)
        {
            Registry.LevelManagerObject.gameObject.SetActive(false); // Enable the level manager object in the scene.
        }

        if (Registry.JoystickObject != null)
        {
            Registry.JoystickObject.gameObject.SetActive(false); // Enable the joystick object in the scene.
        }

        if (Registry.UIManagerObject != null)
        {
            Registry.UIManagerObject.gameObject.SetActive(false); // Enable the UI manager object in the scene.
        }
    }

    private void EnableGameLevelContents()
    {
        foreach (ForegroundCustomer Customer in Registry.ForegroundCustomers)
        {
            Customer.gameObject.SetActive(true); // Enable all foreground customers in the scene.
        }

        foreach (BackgroundCustomer Customer in Registry.BackgroundCustomers)
        {
            Customer.gameObject.SetActive(true); // Enable all background customers in the scene.
        }

        if (Registry.PlayerObject != null) // Check and destroy (garbage collect) the player.
        {
            Registry.PlayerObject.gameObject.SetActive(true); // Enable the player object in the scene.
        }

        if (Registry.LevelManagerObject != null) // Check and destroy (garbage collect) the level manager.
        {
            Registry.LevelManagerObject.gameObject.SetActive(true); // Enable the level manager object in the scene.
        }

        if (Registry.JoystickObject != null) // Check and destroy (garbage collect) the joystick.
        {
            Registry.JoystickObject.gameObject.SetActive(true); // Enable the joystick object in the scene.
        }

        if (Registry.UIManagerObject != null) // Check and destroy (garbage collect) the UI manager.
        {
            Registry.UIManagerObject.gameObject.SetActive(true); // Enable the UI manager object in the scene.
        }
    }

    public void AfterSceneChange() // Runs from the camera one frame after the scene has changed.
    {
        string sceneName = Registry.CurrentSceneName;

        if (sceneName == Constants.GAME_LEVEL) // Check if in a game scene.
        {
            if (SFXSource.clip != RestaurantAmbience)
            {
                SFXSource.clip = RestaurantAmbience; // Set the sound effect to the right track
            }
            SFXSource.volume = Registry.SFXVolume; // Set the volume
            if (!SFXSource.isPlaying)
            {
                SFXSource.Play(); // Start playing the sound if it isn't currently playing.
            }

            if (musicSource.clip != bgm_InGame)
            {
                musicSource.clip = bgm_InGame;
                musicSource.volume = Registry.MusicVolume;
                musicSource.Play();
            }

            EnableGameLevelContents(); // Enable the game level contents.
        }
        else
        {
            DisableGameLevelContents(); // Disable the game level contents.
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
        // End - this section of code was worked on by Joshua Cossar

        if (sceneName == Constants.END_MENU)
        {
            ResetGameLevel(); // When the current scene is the end menu, reset the game (as they cant return to the game scene).
        }

        if (Registry.PlayerObject != null) // Tells the player that the scene has changed.
        {
            Registry.PlayerObject.SceneChanged = true;
        }

        if (Registry.UIManagerObject != null)
        {
            Registry.UIManagerObject.OnSceneChanged();
        }
    }

    public void QuitGame() // Handle what the game should do on exit.
    {
        savedDataManager.Save(); // Save the game configuration.
        Application.Quit(); // Quit the game.
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        Registry.GameInBackground = !hasFocus; // Determine if the game is in the background.
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

    public void ChangeScene(string sceneName = "") // Used to change scene, or to go back to previous scene if no argument provided
    {
        sceneName = HandleSceneStack(sceneName); // Handles determining the previous scene.

        if (sceneName == Constants.GAME_LEVEL)
        {
            if (Registry.UIManagerObject != null)
            {
                Registry.UIManagerObject.gameObject.SetActive(true); // Enable the UI on the scenes it should be on.
            }
        }
        else
        {
            if (SFXSource.clip == RestaurantAmbience)
            {
                SFXSource.Stop(); // If not in the game scenes, stop playing associated ambient sounds
            }

            if (Registry.UIManagerObject != null)
            {
                Registry.UIManagerObject.gameObject.SetActive(false); // Hide the UI
            }

            if (Registry.CurrentSceneName == Constants.GAME_LEVEL)
            {
                RenderGameSceneToFrameBuffer(); // If previously in the game scene, render the blurred background.
            }
        }

        if (sceneName == Constants.MAIN_MENU)
        {
            ResetGameLevel(); // Reset game level when on main menu.
        }

        Registry.CurrentSceneName = sceneName; // keep track of the current scene name

        SceneManager.LoadScene(sceneName); // Change the scene.
    }

    private void Update()
    {
        // Apply the volume adjustments from the options menu
        musicSource.volume = Registry.MusicVolume;
        SFXSource.volume = Registry.SFXVolume;

        // Automatically switch from game scene to end menu when timer runs out (also handled here).
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
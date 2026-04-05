using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class CoreGameInfrastructure : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource MenuMusicSource;

    public AudioSource GameMusicSource;
    public AudioSource MiniGameMusicSource;
    public AudioSource RestaurantAmbienceSource;
    public AudioSource SFXSource;

    [Header("SFX")]
    public OneShotSetup ButtonClickSound;

    [Header("Blurred Background")]
    public Material BlurredMaterial;

    [HideInInspector] public Texture2D FrameTexture;

    [HideInInspector] public Texture2D BlurredFrameTexture;

    private SavedDataManager savedDataManager;

    [Header("Navigable Menus")]
    public GameObject IntroSequenceMenuPrefab;

    private void Awake()
    {
        if (Registry.CGI == null)
        {
            DontDestroyOnLoad(gameObject);
            Registry.CGI = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        savedDataManager = new SavedDataManager();
        savedDataManager.Load();

        Application.targetFrameRate = Mathf.Max(60, (int)Screen.currentResolution.refreshRateRatio.value);
        Instantiate(IntroSequenceMenuPrefab);
    }

    private void Update()
    {
        MenuMusicSource.volume = Registry.MusicVolume;
        GameMusicSource.volume = Registry.MusicVolume;
        MiniGameMusicSource.volume = Registry.MusicVolume;
        SFXSource.volume = Registry.SFXVolume;
        // Restaurant DELIBERATELY ignored here!
    }

    public void PlayClickSound()
    {
        PlayExtendedOneShot(ButtonClickSound);
    }

    public GameObject PlayExtendedOneShot(OneShotSetup Setup)
    {
        GameObject NewAudioSource = new GameObject("SFX_ExtendedOneShot");
        AudioSource ExtendedAudioSourceComponent = NewAudioSource.AddComponent<AudioSource>();
        ExtendedOneShot ExtendedOneShotComponent = NewAudioSource.AddComponent<ExtendedOneShot>();
        ExtendedAudioSourceComponent.clip = Setup._AudioClip;
        ExtendedAudioSourceComponent.volume = Registry.SFXVolume * Setup.Volume;
        ExtendedAudioSourceComponent.panStereo = Setup.StereoPan;
        ExtendedAudioSourceComponent.pitch = Random.Range(1.0f - Setup.PitchRange, 1.0f + Setup.PitchRange);
        ExtendedAudioSourceComponent.Play();
        ExtendedOneShotComponent.Lifetime = Setup._AudioClip.length;
        NewAudioSource.transform.parent = SFXSource.transform;
        return NewAudioSource;
    }

    public void SetupLevelOne()
    {
        Registry.LevelRunTime = Constants.LEVEL_ONE_DURATION;
        Registry.LevelNumber = Constants.LEVEL_ONE;
    }

    public void SetupLevelTwo()
    {
        Registry.LevelRunTime = Constants.LEVEL_TWO_DURATION;
        Registry.LevelNumber = Constants.LEVEL_TWO;
    }

    public void QuitGame()
    {
        savedDataManager.Save();
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
        List<GameObject> UI_GameObjects = new List<GameObject>();
        if (Registry.InGame)
        {
            GameObject[] AllGameObjects = FindObjectsByType<GameObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            foreach (GameObject gameObject in AllGameObjects)
            {
                if (gameObject.layer == LayerMask.NameToLayer("UI"))
                {
                    UI_GameObjects.Add(gameObject);
                    gameObject.SetActive(false);
                }
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

        if (Registry.InGame)
        {
            foreach (GameObject gameObject in UI_GameObjects)
            {
                gameObject.SetActive(true); // Re-enable the UI so it can be properly set-up for the pause menu.
            }
        }

        camera.targetTexture = null; // reset the camera's render target
        Graphics.SetRenderTarget(null); // reset graphics' render target
    }
}

[System.Serializable]
public class OneShotSetup
{
    public AudioClip _AudioClip;
    [Range(0.0f, 1.0f)] public float Volume = 1.0f;
    [Range(-1.0f, 1.0f)] public float StereoPan = 0.0f;
    [Range(0.0f, 0.15f)] public float PitchRange = 0.15f;
}
using UnityEditor;
using UnityEngine;

public class AudioManager : MonoBehaviour
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


    private void Awake()     /*check if audiomanager is already in the scene to avoid duplication*/
    {
        if (Registry.AudioManager == false)
        {
            DontDestroyOnLoad(gameObject);
            Registry.AudioManager = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicSource.clip = bgm_MainMenu;
        musicSource.volume = Registry.MusicVolume;
        musicSource.Play();
    }

    private void Update()
    {
        musicSource.volume = Registry.MusicVolume;
    }
}
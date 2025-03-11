using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("AudioSources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("AudioClips")]
    public AudioClip bgm;
    public AudioClip footsteps;
    public AudioClip audioClip2;
    public AudioClip audioClip3;
    public AudioClip audioClip4;

    private void Awake()
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
        musicSource.clip = bgm;
        musicSource.Play();
    }

}

using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource Source;
    [SerializeField] AudioClip[] music;
    [SerializeField] AudioMixer audioMixer;
    static bool AudioPlayer = false;

    int musicIndex;

    private void Awake()
    {
        if (AudioPlayer == true)
        {
            Source.gameObject.SetActive(false);
        }

        else
        {
            DontDestroyOnLoad(this);
            AudioPlayer = true;
        }
    }

    void Start()
    {
        playRandom();

        //Main Menu sets should be in a seperate script but lazy
        audioMixer.SetFloat("volume", PlayerPrefs.GetFloat("musicVolume"));
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("qualityInt"));
        int fullSave = PlayerPrefs.GetInt("fullScreen");

        if (fullSave == 1)
        {
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
        }
    }

    private void FixedUpdate()
    {
        if (!Source.isPlaying)
        {
            playRandom();

        }
    }

    private void playRandom()
    {
        musicIndex = Random.Range(0, music.Length);
        Source.clip = music[musicIndex];
        Source.Play();
    }
}

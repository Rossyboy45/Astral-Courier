using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource Source;
    [SerializeField] AudioClip[] music;
    int musicIndex;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        playRandom();
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

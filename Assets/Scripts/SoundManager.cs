using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    public static SoundManager Instance { get { return _instance; } }

    public AudioClip menuSound;
    public AudioClip gameSound;
    public AudioClip bossSound;

    private AudioSource audioSource;

    private bool muted;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        muted = false;

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    public void ToggleSound(bool mute)
    {
        muted = mute;
        if (muted)
            audioSource.Stop();
        else
            audioSource.Play();
    }

    public void PlayMenuSound()
    {
        if (!muted)
        {
            audioSource.clip = menuSound;
            audioSource.Play();
        }
    }

    public void PlayGameSound()
    {
        if (!muted)
        {
            audioSource.clip = gameSound;
            audioSource.Play();
        }
    }

    public void PlayBossSound()
    {
        if (!muted)
        {
            audioSource.clip = bossSound;
            audioSource.Play();
        }
    }


}

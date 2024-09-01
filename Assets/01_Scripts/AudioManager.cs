using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicAS;
    public AudioSource sfxAS;
    public float sfxVol = 1.0f;
    public float musicVol = 0.5f;


    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlaySFX(AudioClip sound)
    {
        //sfxAS.PlayDelayed(0.1f);
        sfxAS.PlayOneShot(sound);
    }
    //public void PlaySFXDelay(AudioClip sound, float timeDelayed)
    //{
    //    timeDelayed = timeDelayed - sound.length;
    //    sfxAS.PlayDelayed(timeDelayed);
    //}

    public void SetMusic(AudioClip music)
    {
        musicAS.Stop();
        musicAS.clip = music;
        musicAS.Play();
    }

    void Start()
    {
        musicAS.volume = musicVol;
        musicAS.loop = true;
        sfxAS.volume = sfxVol;
        sfxAS.loop = true;
    }

    void Update()
    {

    }
}

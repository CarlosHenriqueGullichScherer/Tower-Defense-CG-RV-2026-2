
using UnityEngine;

[System.Serializable]
public class SoundVolume
{
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 1f;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField, Range(0f, 1f)] private float musicVolume = 0.3f;

    [Header("Sound Effects")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private SoundVolume[] soundVolumes;

    [Header("Game Sounds")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private AudioClip defeatSound;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        musicSource.volume = musicVolume;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        float volume = 1f;

        foreach (SoundVolume sound in soundVolumes)
        {
            if (sound.clip == clip)
            {
                volume = sound.volume;
                break;
            }
        }

        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayBackgroundMusic()
    {
        PlayMusic(backgroundMusic);
    }

    public void PlayVictory()
    {
        StopMusic();
        PlaySFX(victorySound);
    }

    public void PlayDefeat()
    {
        StopMusic();
        PlaySFX(defeatSound);
    }
}

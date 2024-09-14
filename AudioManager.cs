using RTLTMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] AudioClip defaultClip;

    [Header("UI")]
    public Slider musicSlider;
    public RTLTextMeshPro musicVolumeTextMesh;
    public Slider sfxSlider;
    public RTLTextMeshPro sfxVolumeTextMesh;

    [Header("Volume Settings")]
    [Range(0f, 1f)]
    public float musicVolume = 1f;
    [Range(0f, 1f)]
    public float sfxVolume = 1f;

    [Header("Pooling")]
    public ObjectPoolManager objectPoolManager;

    [SerializeField] AudioObject musicObject;
    [SerializeField] AudioClip defaultMusic;
    private AudioClip musicClip;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of AudioManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Load saved volume settings
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        if (musicSlider != null)
        {
            musicSlider.value = musicVolume;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            musicVolumeTextMesh.text = Mathf.RoundToInt(musicVolume * 100).ToString() + "%";
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = sfxVolume;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
            sfxVolumeTextMesh.text = Mathf.RoundToInt(sfxVolume * 100).ToString() + "%";
        }

        PlayMusic(defaultMusic);
    }

    private void Start()
    {
        UpdateAudioSources();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();
        UpdateAudioSources();
        musicVolumeTextMesh.text = Mathf.RoundToInt(volume * 100).ToString() + "%";
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
        UpdateAudioSources();
        PlayAudio(defaultClip);
        sfxVolumeTextMesh.text = Mathf.RoundToInt(volume * 100).ToString() + "%";
    }

    private void UpdateAudioSources()
    {
        // Update all audio sources in the scene
        AudioObject[] audioObjects = FindObjectsOfType<AudioObject>();
        foreach (AudioObject audio in audioObjects)
        {
            audio.UpdateVolume();
        }
    }

    public void PlayAudio(AudioClip clip, float pitch = 1.0f, Vector3 position = default, AudioObject.AudioType audioType = AudioObject.AudioType.SFX)
    {
        GameObject sfxObject = objectPoolManager.GetObject();
        sfxObject.transform.position = position;

        AudioObject audioObject = sfxObject.GetComponent<AudioObject>();
        audioObject.audioSource.clip = clip;
        audioObject.audioType = audioType;
        audioObject.UpdateVolume();
        audioObject.audioSource.pitch = pitch;
        audioObject.audioSource.Play();

        StartCoroutine(ReturnToPoolAfterPlaying(audioObject.audioSource));
    }

    private IEnumerator ReturnToPoolAfterPlaying(AudioSource audioSource)
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        objectPoolManager.ReleaseObject(audioSource.gameObject);
    }

    public IEnumerator PlayAudioWithDelay(AudioClip clip, float delay = 0, float pitch = 1.0f, Vector3 position = default, AudioObject.AudioType audioType = AudioObject.AudioType.SFX)
    {
        yield return new WaitForSeconds(delay);
        PlayAudio(clip, pitch, position, audioType);
    }

    public void PlayMusic(AudioClip newMusic)
    {
        if (newMusic == null)
        {
            newMusic = defaultMusic;
        }

        if (musicObject.audioSource.clip == newMusic)
        {
            return;
        }

        StartCoroutine(FadeMusic(newMusic));
    }

    private IEnumerator FadeMusic(AudioClip newMusic)
    {
        if (musicObject.audioSource.isPlaying)
        {
            // Fade out current music
            yield return StartCoroutine(FadeOut(musicObject.audioSource, 0.5f));
        }

        musicClip = newMusic;
        musicObject.audioSource.clip = newMusic;
        musicObject.audioSource.Play();

        // Fade in new music
        yield return StartCoroutine(FadeIn(musicObject.audioSource, 0.5f));
    }

    private IEnumerator FadeOut(AudioSource audioSource, float fadeDuration)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t <= fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0;
        audioSource.Stop();
    }

    private IEnumerator FadeIn(AudioSource audioSource, float fadeDuration)
    {
        audioSource.volume = 0;
        audioSource.Play();

        for (float t = 0; t <= fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, musicVolume, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = musicVolume;
    }
    
}

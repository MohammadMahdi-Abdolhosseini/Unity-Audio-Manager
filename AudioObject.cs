using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioObject : MonoBehaviour
{
    public enum AudioType
    {
        Music,
        SFX
    }

    public AudioType audioType;
    public AudioSource audioSource;

    public void UpdateVolume()
    {
        if (audioSource == null) return;

        switch (audioType)
        {
            case AudioType.Music:
                audioSource.volume = AudioManager.Instance.musicVolume;
                break;
            case AudioType.SFX:
                audioSource.volume = AudioManager.Instance.sfxVolume;
                break;
        }
    }
}

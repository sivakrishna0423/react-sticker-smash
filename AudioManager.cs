using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<LocalizedAudioClips> localizedAudioClips;
    
    private int selectedLanguageIndex = 0;

    public void Initialize()
    {
        selectedLanguageIndex = LanguageSelection.Instance.LanguageSelected;
        
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public IEnumerator PlayWelcomeAudio()
    {
        var clips = GetLocalizedClips()?.welcomeAudio;
        if (clips == null) yield break;

        for (int i = 0; i < clips.Length; i++)
        {
            yield return PlayAudioClip(clips[i]);
        }
    }

    public IEnumerator PlayVideoAudio()
    {
        var clip = GetLocalizedClips()?.videoAudioClip;
        if (clip != null)
        {
            yield return PlayAudioClip(clip);
        }
    }

    public void PlayHoldVoiceOver(int playCount)
    {
        var holdVoiceOver = GetLocalizedClips()?.holdVoiceOver;
        if (holdVoiceOver != null && playCount > 1)
        {
            PlayAudioClip(holdVoiceOver);
        }
    }

    public void PlayRelaxAudio()
    {
        var relaxAudio = GetLocalizedClips()?.relaxAudio;
        if (relaxAudio != null)
        {
            PlayAudioClip(relaxAudio);
        }
    }

    public void StopAudio()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    private IEnumerator PlayAudioClip(AudioClip clip)
    {
        if (clip == null || audioSource == null) yield break;

        audioSource.clip = clip;
        audioSource.Play();
        yield return new WaitForSeconds(clip.length);
    }

    private void PlayAudioClip(AudioClip clip)
    {
        if (clip == null || audioSource == null) return;

        audioSource.clip = clip;
        audioSource.Play();
    }

    private LocalizedAudioClips GetLocalizedClips()
    {
        if (localizedAudioClips == null || selectedLanguageIndex >= localizedAudioClips.Count)
            return null;

        return localizedAudioClips[selectedLanguageIndex];
    }
}

[System.Serializable]
public class LocalizedAudioClips
{
    public string languageName; // e.g., "English", "Hindi", "Spanish"
    public AudioClip[] welcomeAudio;
    public AudioClip videoAudioClip;
    public AudioClip relaxAudio;
    public AudioClip holdVoiceOver;
}
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    [Tooltip("인스펙터에서 설정하는 배경음악 볼륨 값 (0~100)")]
    [SerializeField] [Range(0, 100)] private int bgmVolume = 50;
    [SerializeField] [Range(0, 100)] private int sfxVolume = 50;
    
    private void OnEnable() 
    {
        ApplyBgmVolume();
        ApplySfxVolume();
    }

    public int GetBgmVolume() => bgmVolume;
    public int GetSfxVolume() => sfxVolume;

    public void SetBgmVolume(int value)
    {
        bgmVolume = Mathf.Clamp(value, 0, 100);
        ApplyBgmVolume();
    }
    
    public void SetSfxVolume(int value)
    {
        bgmVolume = Mathf.Clamp(value, 0, 100);
        ApplySfxVolume();
    }

    private void ApplyBgmVolume()
    {
        if (bgmAudioSource == null)
        {
            Debug.LogWarning("AudioSettingsManager : BGM AudioSource가 연결되지 않았습니다.");
            return;
        }
        bgmAudioSource.volume = bgmVolume / 100f;
    }
    
    private void ApplySfxVolume()
    {
        if (sfxAudioSource == null)
        {
            Debug.LogWarning("AudioSettingsManager : BGM AudioSource가 연결되지 않았습니다.");
            return;
        }
        sfxAudioSource.volume = sfxVolume / 100f;
    }
    public void OnBgmPlay(AudioClip clip)
    {
        if (clip == null) return;
        bgmAudioSource.clip = clip;
        bgmAudioSource.Play();
    }

    public void OnSfxPlayOnShot(AudioClip clip)
    {
        if (clip != null) sfxAudioSource.PlayOneShot(clip);   
    }

    public void OnSfxPlay(AudioClip clip)
    {
        if (clip == null) return;
        sfxAudioSource.clip = clip;
        sfxAudioSource.Play();
    }
}
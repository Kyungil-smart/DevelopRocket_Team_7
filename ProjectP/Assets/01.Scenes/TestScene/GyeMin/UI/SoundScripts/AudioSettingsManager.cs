using UnityEngine;

public class AudioSettingsManager : MonoBehaviour
{
    public static AudioSettingsManager Instance;

    [Header("배경음악 설정")]

    [Tooltip("배경음악을 재생하는 AudioSource")]
    [SerializeField] private AudioSource bgmAudioSource;

    [Tooltip("인스펙터에서 설정하는 배경음악 볼륨 값 (0~100)")]
    [Range(0, 100)]
    [SerializeField] private int bgmVolume = 50;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        ApplyBgmVolume();
    }


    public int GetBgmVolume()
    {
        return bgmVolume;
    }


    public void SetBgmVolume(int value)
    {
        bgmVolume = Mathf.Clamp(value, 0, 100);
        ApplyBgmVolume();
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
}
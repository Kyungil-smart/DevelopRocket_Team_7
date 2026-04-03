using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundUIController : MonoBehaviour
{
    [Header("사운드 관련 UI 연결")]

    [Tooltip("사운드 관련 볼륨 조절 슬라이더")]
    [SerializeField] private Slider soundSlider;

    [Tooltip("사운드 관련 볼륨 숫자 표시 텍스트")]
    [SerializeField] private TMP_Text soundValueText;
    
    [SerializeField] private SoundType soundType;


    private void Start()
    {
        soundSlider.minValue = 0;
        soundSlider.maxValue = 100;
        soundSlider.wholeNumbers = true;

        int savedVolume = 50;

        if (AudioManager.Instance != null)
        {
            if (soundType == SoundType.BGM) savedVolume = AudioManager.Instance.GetBgmVolume();
            else savedVolume = AudioManager.Instance.GetSfxVolume();
        }

        soundSlider.SetValueWithoutNotify(savedVolume);
        UpdateVolumeText(savedVolume);

        soundSlider.onValueChanged.AddListener(OnChangedSoundSlider);
    }


    private void OnDestroy()
    {
        if (soundSlider != null)
        {
            soundSlider.onValueChanged.RemoveListener(OnChangedSoundSlider);
        }
    }


    private void OnChangedSoundSlider(float value)
    {
        int intValue = Mathf.RoundToInt(value);
        UpdateVolumeText(intValue);
        if (AudioManager.Instance != null)
        {
            if (soundType == SoundType.BGM) AudioManager.Instance.SetBgmVolume(intValue);
            else AudioManager.Instance.SetSfxVolume(intValue);
        }
    }


    private void UpdateVolumeText(int value)
    {
        soundValueText.text = value.ToString();
    }
}
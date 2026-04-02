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


    private void Start()
    {
        if (soundSlider == null)
        {
            Debug.LogError("BgmSettingUI : bgmSlider가 연결되지 않았습니다.");
            return;
        }

        if (soundValueText == null)
        {
            Debug.LogError("BgmSettingUI : bgmValueText가 연결되지 않았습니다.");
            return;
        }

        soundSlider.minValue = 0;
        soundSlider.maxValue = 100;
        soundSlider.wholeNumbers = true;

        int savedVolume = 50;

        if (AudioSettingsManager.Instance != null)
        {
            savedVolume = AudioSettingsManager.Instance.GetBgmVolume();
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

        if (AudioSettingsManager.Instance != null)
        {
            AudioSettingsManager.Instance.SetBgmVolume(intValue);
        }
    }


    private void UpdateVolumeText(int value)
    {
        soundValueText.text = value.ToString();
    }
}
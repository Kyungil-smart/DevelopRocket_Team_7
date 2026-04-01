using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BgmSettingUI : MonoBehaviour
{
    [Header("배경음악 UI 연결")]

    [Tooltip("배경음악 볼륨 조절 슬라이더")]
    [SerializeField] private Slider bgmSlider;

    [Tooltip("배경음악 볼륨 숫자 표시 텍스트")]
    [SerializeField] private TMP_Text bgmValueText;


    private void Start()
    {
        if (bgmSlider == null)
        {
            Debug.LogError("BgmSettingUI : bgmSlider가 연결되지 않았습니다.");
            return;
        }

        if (bgmValueText == null)
        {
            Debug.LogError("BgmSettingUI : bgmValueText가 연결되지 않았습니다.");
            return;
        }

        bgmSlider.minValue = 0;
        bgmSlider.maxValue = 100;
        bgmSlider.wholeNumbers = true;

        int savedVolume = 50;

        if (AudioSettingsManager.Instance != null)
        {
            savedVolume = AudioSettingsManager.Instance.GetBgmVolume();
        }

        bgmSlider.SetValueWithoutNotify(savedVolume);
        UpdateVolumeText(savedVolume);

        bgmSlider.onValueChanged.AddListener(OnChangedBgmSlider);
    }


    private void OnDestroy()
    {
        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged.RemoveListener(OnChangedBgmSlider);
        }
    }


    private void OnChangedBgmSlider(float value)
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
        bgmValueText.text = value.ToString();
    }
}
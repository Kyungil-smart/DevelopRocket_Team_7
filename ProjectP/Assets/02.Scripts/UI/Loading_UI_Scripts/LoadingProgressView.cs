using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingProgressView : MonoBehaviour
{
    [Header("게이지 UI")]

    [Tooltip("로딩 진행도를 표시할 Slider")]
    [SerializeField] private Slider loadingSlider;

    [Tooltip("게이지 안에 표시할 퍼센트 텍스트")]
    [SerializeField] private TextMeshProUGUI percentText;


    public void Initialize()
    {
        if (loadingSlider != null)
        {
            loadingSlider.minValue = 0;
            loadingSlider.maxValue = 100;
            loadingSlider.wholeNumbers = true;
            loadingSlider.value = 0;
        }

        if (percentText != null)
        {
            percentText.text = "0%";
        }
    }


    public void SetProgress(int percent)
    {
        int clampedPercent = Mathf.Clamp(percent, 0, 100);

        if (loadingSlider != null)
        {
            loadingSlider.value = clampedPercent;
        }

        if (percentText != null)
        {
            percentText.text = clampedPercent + "%";
        }
    }
}
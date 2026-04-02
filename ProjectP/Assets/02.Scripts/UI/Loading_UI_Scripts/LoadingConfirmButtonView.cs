using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingConfirmButtonView : MonoBehaviour
{
    [Header("확인 버튼 참조")]

    [Tooltip("100% 완료 후 눌릴 확인 버튼")]
    [SerializeField] private Button confirmButton;

    [Tooltip("확인 버튼 아이콘 또는 배경 Image")]
    [SerializeField] private Image confirmButtonImage;

    [Tooltip("확인 버튼 안에 들어있는 TMP 텍스트 (아이콘 버튼이면 비워도 됨)")]
    [SerializeField] private TextMeshProUGUI confirmButtonText;


    [Header("버튼 색상")]

    [Tooltip("비활성 상태일 때 버튼 이미지 색상")]
    [SerializeField] private Color disabledButtonColor = new Color(0.45f, 0.45f, 0.45f, 1f);

    [Tooltip("활성 상태일 때 버튼 이미지 색상")]
    [SerializeField] private Color enabledButtonColor = Color.white;

    [Tooltip("비활성 상태일 때 글자색")]
    [SerializeField] private Color disabledTextColor = new Color(0.75f, 0.75f, 0.75f, 1f);

    [Tooltip("활성 상태일 때 글자색")]
    [SerializeField] private Color enabledTextColor = Color.white;


    private void Awake()
    {
        Initialize();
    }


    public void Initialize()
    {
        SetState(false);
    }


    public void SetState(bool isEnabled)
    {
        if (confirmButton != null)
        {
            confirmButton.interactable = isEnabled;
        }

        if (confirmButtonImage != null)
        {
            confirmButtonImage.color = isEnabled ? enabledButtonColor : disabledButtonColor;
        }

        if (confirmButtonText != null)
        {
            confirmButtonText.color = isEnabled ? enabledTextColor : disabledTextColor;
        }
    }
}
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("호버 처리할 부모 오브젝트")]
    [SerializeField] private GameObject _hoverObject;

    public void OnEnable()
    {
        // UI 창 켰을 때 호버창이 켜져 있을 경우를 대비하여 off
        _hoverObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 아이템 정보창 보여주기
        _hoverObject.SetActive(true);
        _hoverObject.transform.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 정보창 닫기
        _hoverObject.SetActive(false);
    }
}

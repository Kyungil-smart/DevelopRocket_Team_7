using UnityEngine;
using UnityEngine.EventSystems;

public class NodeHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("호버 처리할 부모 오브젝트")]
    [SerializeField] private GameObject _hoverObject;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 아이템 정보창 보여주기
        _hoverObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 정보창 닫기
        _hoverObject.SetActive(false);
    }
}

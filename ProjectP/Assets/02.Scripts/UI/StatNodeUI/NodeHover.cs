using UnityEngine;
using UnityEngine.EventSystems;

public class NodeHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 아이템 정보창 보여주기
        Debug.Log("마우스 호버");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 정보창 닫기
        Debug.Log("마우스 나감");
    }
}

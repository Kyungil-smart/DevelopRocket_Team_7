using UnityEngine;

public class OnInteraction : MonoBehaviour, IinteractiveObject
{
    public int index;
    public int Interact()
    {
        /*
        프리팹에 붙은 이 스크립트에 index값을 설정 후
        플레이어가 상호작용 가능한 오브젝트에 해당 스크립트를 붙인다.
        리턴 값(index)을 플레이어 가 받아서 받은 인덱스로
        플레이어 쪽에서 분기 처리
         */
        return index;
    }
}

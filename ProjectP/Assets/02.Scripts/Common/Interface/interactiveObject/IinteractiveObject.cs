using UnityEngine;

public interface IinteractiveObject
{

    /*
     플레이어가 특정 키를 눌렀을 때 
     상호 작용 가능한 오브젝트면 해당 인터페이스 를 구현한 로직 실행

      게임 오브젝트 매개변수가 있는데 이건 플레이어 본인 을 넘길거 같다.  
      상호작용 가능한 오브젝트에서 플레이어가 풀피로 회복 관련 처리할 때
     플레이어 붙은 인터페이스 통해서 회복 처리 할 예정이다.  

      플레이어가 풀피로 회복 관련 인터페이스는 아직 미구현 상태
     */
    public void Interact(GameObject Obj);
    
}

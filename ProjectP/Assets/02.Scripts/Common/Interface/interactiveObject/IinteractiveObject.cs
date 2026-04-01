using UnityEngine;

public interface IinteractiveObject
{
    //플레이어가 특정 키를 눌렀을 때 
    //상호 작용 가능한 오브젝트면 해당 인터페이스 를 구현한 로직 실행

    public void Interact(GameObject Obj);
    
}

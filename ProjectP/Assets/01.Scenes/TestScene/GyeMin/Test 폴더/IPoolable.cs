using UnityEngine;

// 풀링 오브젝트가 생성/비활성화될 때 자동으로 초기화하기 위한 인터페이스
public interface IPoolable
{
    // 풀에서 꺼내질 때 호출
    void OnSpawn();

    // 풀로 반환될 때 호출
    void OnDespawn();
}

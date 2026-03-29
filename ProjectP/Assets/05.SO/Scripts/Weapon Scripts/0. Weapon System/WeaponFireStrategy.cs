using UnityEngine;

public abstract class WeaponFireStrategy : ScriptableObject
{
    // 구현 원리 요약:
    // 무기마다 발사 방식이 다르므로 공통 인터페이스로 분리
    public abstract void Fire(Transform firePoint, WeaponBlackboard data);
}
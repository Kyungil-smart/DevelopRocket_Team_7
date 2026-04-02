
// 공격 종류를 구분해서 몬스터가 처리 로직을 분기하기 위한 타입

public enum DamageType
{
    Projectile,   // 투사체 (총알, 샷건, 스나이퍼)
    Explosion,    // 범위 폭발 (고무탄 등)
    Hitscan,      // 즉시 판정 (레이저)
    Melee         // 근접 공격 (건틀릿)
}
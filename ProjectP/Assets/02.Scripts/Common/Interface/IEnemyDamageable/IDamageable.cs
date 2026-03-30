// 몬스터가 데미지를 받을 수 있도록 인터페이스 정의

public interface IDamageable
{
    void TakeDamage(DamageType type, int damage);
}
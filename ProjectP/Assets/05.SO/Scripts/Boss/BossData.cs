using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName = "Data/BossData")]
public class BossData : ScriptableObject
{
    public float maxHp;
    public int damage;
    public int energyBulletDamage;
    public float speed;
    public float attackRange;
    public float detectRadius;
    public float attackDelay;
    public float batteryProbability;
    public float speedRateOnBurning;
}
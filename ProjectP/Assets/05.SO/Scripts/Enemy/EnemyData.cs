using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/MonsterData")]
public class EnemyData : ScriptableObject
{
    public int maxHp;
    public int damage;
    public float speed;
    public float attackRange;
    public float detectRadius;
    public float attackDelay;
    public float batteryProbability;
}

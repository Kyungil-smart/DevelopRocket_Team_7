using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/MonsterData")]
public class EnemyData : ScriptableObject
{
    public string name;
    public int maxHp;
    public float damage;
    public float speed;
    public float attackRange;
    public float detectRadius;
}

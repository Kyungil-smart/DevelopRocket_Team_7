using UnityEngine;

[CreateAssetMenu(fileName = "BatteryData", menuName = "Data/BatteryData")]
public class BatteryStatus : ScriptableObject
{
    public int textid;
    [Range(0f, 1f)] public float attackSpeed;
    [Range(0f, 1f)] public float damage;  
    [Range(0f, 1f)] public float critRate;
    [Range(0f, 1f)] public float critMultiplier;
}
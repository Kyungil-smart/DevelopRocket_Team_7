using UnityEngine;

[CreateAssetMenu(menuName = "Data/Weapon/Shotgun")]
public class ShotgonData : ScriptableObject
{
    [Header("샷건 설정")]
    [Tooltip("샷건 펠렛 개수")] public int pelletCount = 5;
    [Tooltip("퍼짐 각도")] public float spreadAngle = 30f;
    // 여러 방향으로 퍼지는 각도 (샷건 전용)
}
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Weapon/Shotgun")]
public class ShotgonData : ScriptableObject
{
    [Header("샷건 설정")]
    [Tooltip("샷건 펠렛 개수")] public int pelletCount = 5;
    [Tooltip("퍼짐 각도")] public float spreadAngle = 30f;
    // 여러 방향으로 퍼지는 각도 (샷건 전용)

    [Header("스나이퍼 설정")]
    [Tooltip("관통 횟수")] public int pierceCount = 3;
    // 적을 몇 번까지 관통할 수 있는지
}
using UnityEngine;

public class BossSoundEffects : MonoBehaviour
{
    [Header("몬스터 사운드 이펙트")] 
    // 보스 체력 50% 진입시 출력 사운드
    [SerializeField] private AudioClip _bossPhaseChangeSound;
    // 보스 뭉둥이 공격시 출력 사운드
    [SerializeField] private AudioClip _bossBaseAttackSound;

    public void Boss50perChange()
    {
        AudioManager.Instance.OnSfxPlayOnShot(_bossPhaseChangeSound);
    }

    public void BossBaseAttackSound()
    {
        AudioManager.Instance.OnSfxPlayOnShot(_bossBaseAttackSound);
    }
}

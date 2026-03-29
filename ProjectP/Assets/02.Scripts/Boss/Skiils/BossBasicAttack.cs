using UnityEngine;

public class BossBasicAttack : MonoBehaviour, INeedBossBlackboard
{
    [SerializeField] GameObject _firstEffect;
    [SerializeField] GameObject _secondEffect;
    [SerializeField] GameObject _thirdEffect;
    private BossBlackBoard _blackboard;

    private void Update()
    {
        ChangeDamageLocate();
    }

    private void ChangeDamageLocate()
    {
        if (_blackboard == null) return;
        Vector2 pos = (Vector2)transform.position + (_blackboard.bodyDirection * 2f);
        _firstEffect.transform.position = pos;
        _secondEffect.transform.position = pos;
    }
    
    // 아래 세 함수는 [BossBasicAttack.anim] 에 Animation Event 로 등록됨.
    public void OnFirstAttack() => _firstEffect.GetComponent<BossHitToPlayer>().OnPlayerHit();
    public void OnSecondAttack() => _secondEffect.GetComponent<BossHitToPlayer>().OnPlayerHit();
    public void OnThirdAttack() => _thirdEffect.GetComponent<BossHitToPlayer>().OnPlayerHit();
    
    public void SetBlackboard(BossBlackBoard blackboard)
    {
        _blackboard = blackboard;
    }
}

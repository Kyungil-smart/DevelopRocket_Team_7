using System;
using System.Collections;
using UnityEngine;

public class BossChangePhase : MonoBehaviour, INeedBossBlackboard
{
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] GameObject _PhaseChangeEffect;
    [SerializeField] CircleCollider2D _collider;
    private BossBlackBoard _blackBoard;
    private GameObject _player;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider =  GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Utils.CompareLayer(other.gameObject.layer, LayerMask.NameToLayer("Player")))
        {
            _player = other.gameObject;
        }
    }

    public void OnBecameInvisible()
    {
        //_blackBoard.IsInvincible = true;
        //Debug.Log("보스 무적 상태로 변경");
    }

    // 아래 함수는 [BossChangePhase.anim] 에 Animation Event 로 등록됨.
    public void OnImpulseWave()
    {
        //_blackBoard.IsInvincible = false;
        //// ToDo. Player 에게 밀림 함수 달라고 해야한다.
        //Debug.Log("보스 무적 상태 해제");
    }

    // 아래 함수는 [BossChangePhase.anim] 에 Animation Event 로 등록됨.
    private void SetAnimationSpeed()
    {
        _animator.speed = 1.1f;
        _blackBoard.speed *= _blackBoard.origin.speedRateOnBurning;
        Debug.Log("보스 화남");
    }

    // 아래 함수는 [BossChangePhase.anim] 에 Animation Event 로 등록됨.
    public void OnChangeColor()
    {
        Debug.Log("보스 색 변경됨");
        _spriteRenderer.color = new Color(1f, 0.588f, 0.588f, 1f);
    }

    public void SetBlackboard(BossBlackBoard blackboard)
    {
        _blackBoard = blackboard;
    }
}

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
        _blackBoard.IsInvincible = true;
    }

    public void OnImpulseWave()
    {
        _blackBoard.IsInvincible = false;
        // ToDo. Player 에게 밀림 함수 달라고 해야한다.
    }

    private void SetAnimationSpeed()
    {
        _animator.speed = 1.1f;
        _blackBoard.speed *= _blackBoard.origin.speedRateOnBurning;
    }

    public void OnChangeColor()
    {
        _spriteRenderer.color = new Color(1f, 0.588f, 0.588f, 1f);
    }

    public void SetBlackboard(BossBlackBoard blackboard)
    {
        _blackBoard = blackboard;
    }
}

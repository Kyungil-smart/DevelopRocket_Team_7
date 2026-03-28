using System;
using UnityEngine;

public class BossChangePhase : MonoBehaviour, INeedBossBlackboard
{
    [SerializeField] Animator _animator;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] GameObject _PhaseChangeEffect;
    private BossBlackBoard _blackBoard;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void OnBecameInvisible()
    {
        _blackBoard.IsInvincible = true;
    }

    public void OnImpulseWave()
    {
        _blackBoard.IsInvincible = false;
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

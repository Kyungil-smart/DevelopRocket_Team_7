using System;
using System.Collections;
using UnityEngine;

public class BulletAnimController : MonoBehaviour
{
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private AnimationClip _hitAnim;
    private Coroutine _hitAnimRoutine;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnHit()
    {
        _animator.SetTrigger("Hit");
        if (_hitAnimRoutine != null) // 방어코드.
        {
            StopCoroutine(_hitAnimRoutine);
            _hitAnimRoutine = null; 
        }
        if (_hitAnimRoutine == null) _hitAnimRoutine = StartCoroutine(HitAnim());
    }

    private IEnumerator HitAnim()
    {
        AudioManager.Instance.OnSfxPlayOnShot(_hitSound);
        yield return new WaitForSeconds(_hitAnim.length);
        PostManager.Instance.Post(PostMessageKey.ProjectileDespawned, gameObject);
        _hitAnimRoutine = null;
    }
}

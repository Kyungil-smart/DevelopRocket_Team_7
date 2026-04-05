using UnityEngine;

public class PlayerLvUpAnimController : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void OnPlayAnimation()
    {
        _animator.SetTrigger("LevelUp");
    }
}
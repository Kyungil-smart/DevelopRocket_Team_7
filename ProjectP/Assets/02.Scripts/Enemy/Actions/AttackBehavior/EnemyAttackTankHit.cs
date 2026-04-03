using System;
using UnityEngine;

public class EnemyAttackTankHit : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    private GameObject _player;
    private SpriteRenderer _renderer;
    private bool _isDamaged;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Utils.CompareLayer(other.gameObject.layer, _layerMask))
        {
            _player = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (Utils.CompareLayer(other.gameObject.layer, _layerMask))
        {
            _player = null;
        }
    }

    public void SetColor(Color color)
    {
        _renderer.color = color;
    }

    public void SetReady()
    {
        _isDamaged = false;
    }
    
    public void OnPlayerHit(int damage)
    {
        if (_player != null)
        {
            if (!_isDamaged)
            {
                _player.GetComponent<IDamage>().TakeDamage(damage);
                _isDamaged = true;
            }
        }
        SetColor(new Color(0.65f, 0, 0, 0));
    }
}

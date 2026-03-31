using System;
using UnityEngine;

public class EnemyAttackTankHit : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    private GameObject _player;
    private SpriteRenderer _renderer;

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
            _player = null;
    }

    public void SetColor(Color color)
    {
        _renderer.color = color;
    }

    public void OnPlayerHit(int damage)
    {
        if (_player != null)
        {
            _player.GetComponent<IDamage>().TakeDamage(damage);
        }
        SetColor(new Color(0.65f, 0, 0, 0));
    }
}

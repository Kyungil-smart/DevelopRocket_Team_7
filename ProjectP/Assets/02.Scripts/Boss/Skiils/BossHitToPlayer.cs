using UnityEngine;

public class BossHitToPlayer : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;
    private GameObject _player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Utils.CompareLayer(collision.gameObject.layer, _playerLayer))
            _player = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Utils.CompareLayer(collision.gameObject.layer, _playerLayer))
            _player = null;
    }

    public void OnPlayerHit()
    {
        if (_player != null)
        {
            _player.GetComponent<IDamage>().TakeDamage(2);
        }
    }
}

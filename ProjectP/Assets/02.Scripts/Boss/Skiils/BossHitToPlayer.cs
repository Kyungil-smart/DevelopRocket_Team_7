using UnityEngine;

public class BossHitToPlayer : MonoBehaviour
{
    private GameObject _player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Utils.CompareLayer(collision.gameObject.layer, LayerMask.NameToLayer("Player")))
            _player = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Utils.CompareLayer(collision.gameObject.layer, LayerMask.NameToLayer("Player")))
            _player = null;
    }

    public void OnPlayerHit()
    {
        if (_player != null)
        {
            // ToDo. Player 에게 데미지를 주는 함수 달라고 해야함.
            Debug.Log("Player hit!!");
        }
    }
}

using UnityEngine;

public class BossRemnant : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("가즈아 가능?");
        if (Utils.CompareLayer(collision.gameObject.layer, _playerLayer))
        {
            Debug.Log("응 가능");
            PostManager.Instance.Post(PostMessageKey.MainUIGameResult, true);
        }
    }
}
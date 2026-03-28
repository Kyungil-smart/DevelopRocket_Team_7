using UnityEngine;

public class BossHitToPlayer : MonoBehaviour
{
    private bool _isPlayerOnHere;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            _isPlayerOnHere = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) 
            _isPlayerOnHere = false;
    }

    public void OnPlayerHit()
    {
        if (_isPlayerOnHere)
        {
            Debug.Log("Player hit!!");
        }
    }
}

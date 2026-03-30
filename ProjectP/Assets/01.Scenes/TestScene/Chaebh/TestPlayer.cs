using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    private void Update()
    {
        PostManager.Instance.Post<Vector2>(PostMessageKey.PlayerPosition, transform.position);
    }
}
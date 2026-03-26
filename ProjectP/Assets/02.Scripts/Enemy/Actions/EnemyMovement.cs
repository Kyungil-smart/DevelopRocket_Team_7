using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
    
    public void Move(Vector2 nextPosition)
    {  // ToDo. 연산된 방향으로만 움직이도록 할 것.
        // Coroutine?
        Debug.Log($"Moving to {nextPosition}");
    }
}

using UnityEngine;

public class EnemyFindTarget
{
    // ToDo. A* 알고리즘으로 타겟과의 위치 파악 및 다음 움직일 포인트 계산. 
    // 계산된 결과값을 Movement 로 전달할 방법 연구 필요.
    // 시간이 없을 경우 NavMesh 로 할 것. 그럴경우 MonoBehavior 로 변경해야함.

    public Vector2 GetNextPosition(Transform target)
    {
        Debug.Log("Get Next Position!!!");
        return Vector2.zero;
    }
}
